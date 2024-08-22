using Newtonsoft.Json;
using PN.SAPB1.API.Models;
using RestSharp;
using System.Net;

namespace PN.SAPB1.API
{
    public class Connection
    {
        public static string url = " https://LENOVOS145:50000/b1s/v1";

        public static string UserName = "manager";

        public static string Password = "update";

        public static string CompanyDB = "SBO_Demo_BR";

        public static string Server = "LENOVOS145";


        public static string SLSession { get; private set; } = "";

        public static async Task<bool> Login()
        {
            try
            {

                Login login = new Login();
               
                login.UserName = UserName;
                login.Password = Password;
                login.CompanyDB = CompanyDB;
                login.Language = "ln_Portuguese_Br";

                ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

                var client = new RestClient(url);
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
