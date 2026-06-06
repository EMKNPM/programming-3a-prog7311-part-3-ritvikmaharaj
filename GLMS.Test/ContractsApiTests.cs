using System.Net;
using System.Net.Http.Json;
using Xunit;
using GLMS.API.Models;

namespace GLMS.Tests
{
    public class ContractsApiTests : IClassFixture<ApiFactory>
    {
        private readonly HttpClient _client;

        public ContractsApiTests(ApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetContracts_Returns200()
        {
            var response = await _client.GetAsync("/api/contracts");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateContract_Then_GetContracts_ReturnsData()
        {
         
            var contract = new Contract
            {
                ClientId = 2,
                ServiceLevel = "Standard",
                Status = ContractStatus.Expired,
                ContractStartDate = DateTime.UtcNow,
                ContractEndDate = DateTime.UtcNow.AddDays(30)
            };

          
            var postResponse =
                await _client.PostAsJsonAsync("/api/contracts", contract);

            var errorText = await postResponse.Content.ReadAsStringAsync();

            Assert.True(
                postResponse.StatusCode == HttpStatusCode.Created,
                $"POST failed: {errorText}"
            );

         
            var getResponse =
                await _client.GetAsync("/api/contracts");

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var json = await getResponse.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(json));
        }
    }
}