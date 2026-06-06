using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GLMS.API.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetRate()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://api.fastforex.io/fetch-one?from=USD&to=EUR"); //api from fast forex
                 
                var data = JsonConvert.DeserializeObject<dynamic>(response);

                if (data == null || data.rates == null || data.rates.ZAR == null)
                {
                    throw new Exception("error");
                }

                return (decimal)data.rates.ZAR;
            }

            catch (Exception ex)
            {
               
                return 16.50m; // using average rate for June
            }
        }
    }
}