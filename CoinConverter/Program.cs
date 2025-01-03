// See https://aka.ms/new-console-template for more information

using System.Net.Http.Json;
using System.Net.NetworkInformation;
using DotNetEnv;

namespace CoinConverter
{
    class APICalls
    {
        static HttpClient client = new();

        public static async Task<HttpContent> GetJSONFromRapidAPIURL(string URL)
        {
            HttpContent content;

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(URL),
                Headers =
                {
                    { "x-rapidapi-key", DotNetEnv.Env.GetString("RAPIDAPIKEY") },
                    { "x-rapidapi-host", "exchangerate-api.p.rapidapi.com" },
                },
            };

            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            content = response.Content;

            return content;
        }
    }

    class CoinConverterCmd
    {
        string coinAbbreviation;
        float quantity;

        public static decimal ConvertFromPrice(decimal price, decimal value)
        {
            return value * price;
        }

        public CoinConverterCmd(string coinAbbreviation = "", float quantity = 0)
        {
            this.coinAbbreviation = coinAbbreviation;
            this.quantity = quantity;
        }

        public class ExchangeRateAPI_JSON
        {
            public required string provider { get; set; }
            public required string documentation { get; set; }
            public required string terms_of_use { get; set; }
            public required int time_last_update_unix { get; set; }
            public required string time_last_update_utc { get; set; }
            public required int time_next_update_unix { get; set; }
            public required string time_next_update_utc { get; set; }
            public required int time_eol_unix { get; set; }
            public required string base_code { get; set; }
            public required Dictionary<string, float> rates { get; set; }
        }

        public float convertFromCoinAbbreviation(string coinAbbreviationLocal)
        {
            if (this.quantity == 0 || this.coinAbbreviation == "")
            {
                throw new Exception("No money quantity or coin abbreviation found");
            }
            else
            {
                float localCoinValue = APICalls
                    .GetJSONFromRapidAPIURL(
                        $"https://exchangerate-api.p.rapidapi.com/rapid/latest/{coinAbbreviationLocal}"
                    )
                    .GetAwaiter()
                    .GetResult()
                    .ReadFromJsonAsync<CoinConverterCmd.ExchangeRateAPI_JSON>()
                    .GetAwaiter()
                    .GetResult()
                    .rates[this.coinAbbreviation];
                return this.quantity * localCoinValue;
            }
        }

        public static void Main(string[] args)
        {
            DotNetEnv.Env.TraversePath().Load();
            CoinConverterCmd Converter = new("BRL", 1);

            Console.WriteLine(
                APICalls
                    .GetJSONFromRapidAPIURL(
                        "https://exchangerate-api.p.rapidapi.com/rapid/latest/USD"
                    )
                    .GetAwaiter()
                    .GetResult()
                    .ReadFromJsonAsync<CoinConverterCmd.ExchangeRateAPI_JSON>()
                    .GetAwaiter()
                    .GetResult()
                    .time_last_update_utc
            );
            Console.WriteLine(Converter.convertFromCoinAbbreviation("USD"));
        }
    }
}
