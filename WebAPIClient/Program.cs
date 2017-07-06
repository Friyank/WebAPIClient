using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApiHelper;

namespace WebAPIClient
{
    class Caller
    {
        private static string serviceUrl = "https://fapple.crm.dynamics.com/";  //CRM user account
        private static string clientId = "1bf3a99f-c5cc-47c4-97cd-3b8e6fc47ed5";
        private static string UserName = "friyank@Fapple.onmicrosoft.com";
        private static string Password = "dell@123";
        private static string baseUrlForAPI = "https://fapple.api.crm.dynamics.com/api/data/v8.2/";

        //uncoomemt follwoing when you want to get data based on save query i.e using views name in CRM
        private static string GenerateUrlForGet = baseUrlForAPI + "savedqueries?$select=name,savedqueryid&$filter=name eq 'My Active Accounts'";
        private static string UrlToGetDataBySavedQuery = baseUrlForAPI + "accounts?savedQuery=";

        //url for Create Contact
        private static string CreateContactUrl = baseUrlForAPI + "contacts";

        // uncomment this to fetch accounts data based on filters
        //private static string GenerateUrlForGet = "api/data/v8.1/accounts?$select=name,revenue&$filter=revenue lt 100000";

        static void Main(string[] args)
        {
            HttpMessageHandler messageHandler;
            //get Handler
            //messageHandler = new AuthenitcateCaller(serviceUrl, clientId, new HttpClientHandler(), UserName, Password, HttpMethod.Get);
            //post Handler
            messageHandler = new AuthenitcateCaller(serviceUrl, clientId, new HttpClientHandler(), UserName, Password, HttpMethod.Post);

            //using (HttpClient httpClient = new HttpClient(messageHandler))
            //{
            //    httpClient.BaseAddress = new Uri(serviceUrl);
            //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    httpClient.DefaultRequestHeaders.Add("Prefer", "odata.maxpagesize=3");
            //    httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            //    httpClient.Timeout = new TimeSpan(0, 2, 0);
            //    //var response = httpClient.GetAsync("api/data/v8.1/accounts?$select=name,revenue&$filter=revenue lt 100000",
            //    //HttpCompletionOption.ResponseHeadersRead).Result;
            //    var response = httpClient.GetAsync(GenerateUrlForGet, HttpCompletionOption.ResponseHeadersRead).Result;
            //    Console.WriteLine(response.StatusCode);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        ///Code for Get Calls
            //        ///retrive data using filters and saved or user queries
            //        //JObject body = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            //        //var response2 = httpClient.GetAsync(UrlToGetDataBySavedQuery + body["value"].First["savedqueryid"], HttpCompletionOption.ResponseHeadersRead).Result;
            //        //body = JObject.Parse(response2.Content.ReadAsStringAsync().Result);
            //        //foreach (var item in body["value"])
            //        //{
            //        //    Console.WriteLine("Your system user ID is: {0}", item["name"]);
            //        //}
            //    }
            //    else
            //    {
            //        Console.WriteLine("The request failed with a status of '{0}'",
            //               response.ReasonPhrase);
            //    }
            //    Console.ReadKey();
            //}

            //Post handler
            CreateContact(messageHandler);
        }

        private static void CreateContact(HttpMessageHandler messageHandler)
        {
            
            JObject CreateContactJson = new JObject();
            CreateContactJson.Add("firstname", "aarti");
            CreateContactJson.Add("lastname", "parikh");

            HttpRequestMessage CreateContactRequest = new HttpRequestMessage(HttpMethod.Post, CreateContactUrl);
            CreateContactRequest.Content = new StringContent(CreateContactJson.ToString(), Encoding.UTF8, "application/json");
            using (HttpClient HTTPClient = new HttpClient(messageHandler))
            {
                HTTPClient.Timeout = new TimeSpan(0, 2, 0);
                HTTPClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                HTTPClient.DefaultRequestHeaders.Add("OData-Version", "4.0");

                Task<HttpResponseMessage> CreateContactResponse = HTTPClient.SendAsync(CreateContactRequest);
                if (CreateContactResponse.Result.StatusCode == HttpStatusCode.NoContent)
                {
                    var result = (string[])CreateContactResponse.Result.Headers.GetValues("OData-EntityId");
                    Console.WriteLine("Record Created With Name Hiya PArikh and GUID = {0}", result[0]);
                    Console.ReadLine();
                }
                
            }
        }
    }
}

