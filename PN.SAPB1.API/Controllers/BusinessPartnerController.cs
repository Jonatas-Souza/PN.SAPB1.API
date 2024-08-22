using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PN.SAPB1.API.Context;
using PN.SAPB1.API.Models;
using RestSharp;
using System.Net;

namespace PN.SAPB1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessPartnerController : ControllerBase
    {
        private readonly AppDbContext _contex;
        public BusinessPartnerController(AppDbContext contex)
        {
            _contex = contex;
        }

        /// <summary>
        /// Retorna a lista de todos os parceiros do banco de dados da API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<BusinessPartners>> Get()
        {
            var partners = _contex.BusinessPartners
                            .Include(bp => bp.BPAddresses)
                            .Include(bp => bp.BPFiscalTaxIDCollection)
                            .ToList();

            if (partners.IsNullOrEmpty())
            {
                return NotFound("Parceiros de negócio não encontratos...");
            }
            return partners;
        }

        /// <summary>
        /// Retorna os dados de um parceiro específico no banco de dados da API
        /// </summary>
        /// <param name="id">Código do parceiro</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "ObterPN")]
        public ActionResult<BusinessPartners> Get(string id)
        {
            var bp = _contex.BusinessPartners.Include(bp => bp.BPAddresses)
                                             .Include(bp => bp.BPFiscalTaxIDCollection)
                                             .FirstOrDefault(x => x.CardCode == id);
            if (bp is null)
            {
                return NotFound();
            }
            return bp;
        }

        /// <summary>
        /// Realiza a busca dos dados do CNPJ informado e depois salva o registro no banco de dados da API e no SAP B1
        /// </summary>
        /// <param name="cnpj">Informe o número CNPJ com ou sem a máscara</param>
        /// <param name="tipo">Informe C para cliente e F para fornecedor</param>
        /// <returns></returns>
        [HttpPost("{cnpj}")]
        public async Task<ActionResult> Post(string cnpj, string tipo)
        {

            if (cnpj == null || tipo == null)
                return BadRequest();

            if (tipo.ToUpper() != "C" && tipo.ToUpper() != "F")
                return BadRequest("Tipo incorreto, informe C para cliente ou F para fornecedor!");

            cnpj = Uri.UnescapeDataString(cnpj);

            cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");

            var client = new RestClient($"https://receitaws.com.br/v1/cnpj/{cnpj}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);

            BusinessPartners bp = new();

            if (response.IsSuccessful)
            {

                try
                {


                    RetornoCnpj emp = new();

                    emp = JsonConvert.DeserializeObject<RetornoCnpj>(response.Content);

                    if (emp == null || emp.cnpj == null)
                    {
                        throw new Exception();
                    }

                    bp.CardCode = tipo + emp.cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
                    bp.CardName = emp.nome;
                    bp.CardType = tipo == "C" ? "cCustomer" : "cSupplier"; ;
                    bp.Phone1 = emp.telefone;
                    bp.EmailAddress = emp.email;

                    string[] strings = emp.logradouro.Split(' ');

                    bp.BPAddresses.Add(new Bpaddress
                    {
                        AddressName = "Entrega",
                        AddressType = "bo_ShipTo",
                        TypeOfAddress = strings[0],
                        Street = emp.logradouro.Replace(strings[0], ""),
                        StreetNo = emp.numero,
                        BuildingFloorRoom = emp.complemento,
                        Block = emp.bairro,
                        ZipCode = emp.cep,
                        City = emp.municipio,
                        State = emp.uf,
                        Country = "BR",
                        BPCode = bp.CardCode

                    });

                    bp.BPAddresses.Add(new Bpaddress
                    {
                        AddressName = "Cobrança",
                        AddressType = "bo_BillTo",
                        TypeOfAddress = strings[0],
                        Street = emp.logradouro.Replace(strings[0], ""),
                        StreetNo = emp.numero,
                        BuildingFloorRoom = emp.complemento,
                        Block = emp.bairro,
                        ZipCode = emp.cep,
                        City = emp.municipio,
                        State = emp.uf,
                        Country = "BR",
                        BPCode = bp.CardCode

                    });

                    bp.BPFiscalTaxIDCollection.Add(new Bpfiscaltaxidcollection
                    {
                        Address = "",
                        AddrType = "bo_ShipTo",
                        TaxId0 = emp.cnpj,
                        BPCode = bp.CardCode
                    });

                    bp.BPFiscalTaxIDCollection.Add(new Bpfiscaltaxidcollection
                    {
                        Address = "Entrega",
                        AddrType = "bo_ShipTo",
                        TaxId0 = emp.cnpj,
                        BPCode = bp.CardCode
                    });

                    bp.BPFiscalTaxIDCollection.Add(new Bpfiscaltaxidcollection
                    {
                        Address = "Cobrança",
                        AddrType = "bo_BillTo",
                        TaxId0 = emp.cnpj,
                        BPCode = bp.CardCode
                    });

                    _contex.BusinessPartners.Add(bp);

                    if (_contex.SaveChanges() > 0)
                    {
                        try
                        {

                            string server = B1Connection.Server;
                            string url = B1Connection.Url;

                            var client2 = new RestClient(url);
                            var request2 = new RestRequest($"/BusinessPartners", Method.POST);

                            var settings = new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            };

                            var body = JsonConvert.SerializeObject(bp, settings);

                            request2.AddParameter("application/json", body, ParameterType.RequestBody);

                            CookieContainer cookiecon = new CookieContainer();
                            cookiecon.Add(new Cookie("B1SESSION", B1Connection.SLSession, "/b1s/v1", server));

                            client2.CookieContainer = cookiecon;

                            RestResponse response2 = (RestResponse)await client2.ExecuteAsync(request2);

                            if (response2.IsSuccessful)
                            {
                                return new CreatedAtRouteResult("ObterPN", new { id = bp.CardCode }, bp);
                            }
                            else
                            {
                                ErrorSL er = JsonConvert.DeserializeObject<ErrorSL>(response2.Content);
                                return StatusCode(500, $"Error SL: ({er.error.code}) - {er.error.message.value}");
                            }

                        }
                        catch (Exception ex)
                        {
                            return StatusCode(500, $"Internal server error: {ex.Message}");
                        }
                    }
                    else
                    {
                        return StatusCode(500, "Error saving to the database.");
                    }

                }
                catch
                {
                    CnpjInvalido invalido = new();

                    invalido = JsonConvert.DeserializeObject<CnpjInvalido>(response.Content);

                    return BadRequest(invalido);
                }
            }

            else //if ((response.StatusCode == HttpStatusCode.TooManyRequests) || (response.StatusCode == HttpStatusCode.GatewayTimeout))
            {
                _429_504 ret = new();

                ret = JsonConvert.DeserializeObject<_429_504>(response.Content);

                return StatusCode((int)response.StatusCode, ret);
            }

        }

        /// <summary>
        /// Salva o parceiro no banco de dados da API e no SAP B1 baseado no Json enviado
        /// </summary>
        /// <param name="bp"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(BusinessPartners bp)
        {

            if (bp == null)
                return BadRequest();


            _contex.BusinessPartners.Add(bp);

            if (_contex.SaveChanges() > 0)
            {
                try
                {

                    string server = B1Connection.Server;
                    string url = B1Connection.Url;

                    var client = new RestClient(url);
                    var request = new RestRequest($"/BusinessPartners", Method.POST);

                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    var body = JsonConvert.SerializeObject(bp, settings);

                    request.AddParameter("application/json", body, ParameterType.RequestBody);

                    CookieContainer cookiecon = new CookieContainer();
                    cookiecon.Add(new Cookie("B1SESSION", B1Connection.SLSession, "/b1s/v1", server));

                    client.CookieContainer = cookiecon;

                    RestResponse response = (RestResponse)await client.ExecuteAsync(request);

                    if (response.IsSuccessful)
                    {

                    }
                    else
                    {
                        ErrorSL er = JsonConvert.DeserializeObject<ErrorSL>(response.Content);
                        throw new WebException($"Error SL: ({er.error.code}) - {er.error.message.value}");
                    }

                }
                catch
                {
                }
            }

            return new CreatedAtRouteResult("ObterPN", new { id = bp.CardCode }, bp);
        }

       
    }
}
