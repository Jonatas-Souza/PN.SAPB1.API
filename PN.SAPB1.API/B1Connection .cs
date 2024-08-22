using Newtonsoft.Json;
using PN.SAPB1.API.Models;
using RestSharp;
using System.Net;

namespace PN.SAPB1.API
{
    public class B1Connection
    {
        public static string Url;

        public static string UserName;
     
        public static string Password;

        public static string CompanyDB;

        public static string Server;

        public static string SLSession;


        public B1Connection(string url, string userName, string password, string companyDB, string server)
        {
            Url = url;
            UserName = userName;
            Password = password;
            CompanyDB = companyDB;
            Server = server;
        }

        public async Task<bool> Login()
        {
            try
            {

                Login login = new Login();
               
                login.UserName = UserName;
                login.Password = Password;
                login.CompanyDB = CompanyDB;
                login.Language = "ln_Portuguese_Br";

                ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

                var client = new RestClient(Url);
                var request = new RestRequest("/Login", Method.POST);

                var body = JsonConvert.SerializeObject(login);

                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                RestResponse response = (RestResponse)await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    SLSession = response.Cookies.FirstOrDefault(x => x.Name == "B1SESSION").Value;
                    return true;
                }
                else
                {
                    ErrorSL er = JsonConvert.DeserializeObject<ErrorSL>(response.Content);
                    throw new WebException($"Error SL: ({er.error.code}) - {er.error.message.value}");
                }

            }
            catch 
            {
                return false;
            }

        }
    }
}
