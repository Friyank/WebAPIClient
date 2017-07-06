using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using WebApiHelper;

namespace WebAPIClient
{
    class Caller
    {
        private static string serviceUrl = "https://fapple.crm.dynamics.com/";  //CRM user account
        private static string clientId = "1bf3a99f-c5cc-47c4-97cd-3b8e6fc47ed5";
        private static string UserName = "friyank@Fapple.onmicrosoft.com";
        private static string Password = "dell@123";

        static void Main(string[] args)
        {
            HttpMessageHandler messageHandler;
            messageHandler = new AuthenitcateCaller(serviceUrl, clientId,
                        new HttpClientHandler(), UserName, Password);

            using (HttpClient httpClient = new HttpClient(messageHandler))
            {
                httpClient.BaseAddress = new Uri(serviceUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Prefer", "odata.maxpagesize=3");
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                //var response = httpClient.GetAsync("api/data/v8.1/accounts?$select=name,revenue&$filter=revenue lt 100000",
                //HttpCompletionOption.ResponseHeadersRead).Result;
                var response = httpClient.GetAsync("api/data/v8.1/contacts?$select=lastname,address1_line1",
                     HttpCompletionOption.ResponseHeadersRead).Result;
                Console.WriteLine(response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    JObject body = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    foreach (var item in body["value"])
                    {
                        Console.WriteLine("Your system user ID is: {0}", item["address1_line1"]);
                        Console.WriteLine("Your system user ID is: {0}", item["lastname"]);
                    }

                }
                else
                {
                    Console.WriteLine("The request failed with a status of '{0}'",
                           response.ReasonPhrase);
                }
                Console.ReadKey();
            }
        }
    }
}
