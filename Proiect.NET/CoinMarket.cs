using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace Proiect.NET
{
    class CoinMarket
    {
        private string API_KEY = "f0f5f963-778c-4cf8-a17f-de8c8054288a";
        public static string[] coins = new string[] { "Bitcoin", "Ethereum", "Litecoin", "TRON", "Neo", "Cardano", "Stellar", "EOS", "Dogecoin", "Tether", "XRP" };
        public string[] ids = { "1", "1027", "2", "1958", "1376", "2010", "512", "1765", "74", "825", "52" };
        private dynamic[] myCoins = new dynamic[11];


        public dynamic[] GetMyCoins()
        {
            return myCoins;
        }

        public string GetCoinDescription(string id)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY);
            var response = httpClient.GetAsync("https://pro-api.coinmarketcap.com/v1/cryptocurrency/info?id=" + id).Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"Failed req {response.StatusCode}");
            }

            string responseMessage = response.Content.ReadAsStringAsync().Result;
            dynamic convertedValue = JsonConvert.DeserializeObject(responseMessage);
            var values = convertedValue["data"][id];
            return values["description"];
        }

        public void UpdateData()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY);
            var response = httpClient.GetAsync("https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest?id=1,1027,2,1958,1376,2010,512,1765,74,825,52").Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"Failed req {response.StatusCode}");
            }

            string responseMessage = response.Content.ReadAsStringAsync().Result;
            dynamic convertedValue = JsonConvert.DeserializeObject(responseMessage);
            var values = convertedValue["data"];
            for (int i = 0; i < ids.Length; i++)
            {
                myCoins[i] = values[ids[i]];
            }
        }
        public CoinMarket()
        {
        }
    }
}
