using System.Net.Http.Json;
using PROGPOEP2.Models;

namespace PROGPOEP2.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;

        public ApiClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Contract>> GetContracts()
        {
            var data = await _httpClient
                .GetFromJsonAsync<List<Contract>>("api/contracts");

            return data ?? new List<Contract>();
        }

        public async Task<List<Contract>> SearchContracts(DateTime? start, DateTime? end, ContractStatus? status)
        {
            var url = "api/contracts/search";

            var query = new List<string>();

            if (start.HasValue)
                query.Add($"start={start.Value:yyyy-MM-dd}");

            if (end.HasValue)
                query.Add($"end={end.Value:yyyy-MM-dd}");

            if (status.HasValue)
                query.Add($"status={status.Value}");

            if (query.Count > 0)
                url += "?" + string.Join("&", query);

            var data = await _httpClient
                .GetFromJsonAsync<List<Contract>>(url);

            return data ?? new List<Contract>();
        }

        public async Task<bool> CreateContract(Contract contract)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/contracts",
                contract
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<List<ServiceRequest>> GetServiceRequests()
        {
            var data = await _httpClient
                .GetFromJsonAsync<List<ServiceRequest>>("api/servicerequests");

            return data ?? new List<ServiceRequest>();
        }

        public async Task<bool> CreateServiceRequest(ServiceRequest request, decimal usdAmount)
        {
            var url = $"api/servicerequests?usdAmount={usdAmount}";

            var response = await _httpClient.PostAsJsonAsync(url, request);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<Client>> GetClients()
        {
            var data = await _httpClient.GetFromJsonAsync<List<Client>>("api/clients");

            return data ?? new List<Client>();
        }
    }
}