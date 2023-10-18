using Api.IntegrationTest2.Attributes;
using Microsoft.Net.Http.Headers;
using Persistence;
using Xunit;

namespace Api.IntegrationTest2.Controllers
{
    [Collection("IntegrationTestBaseCollection")]
    [TestCaseOrderer(
        ordererTypeName: "Api.IntegrationTest2.Attributes.PriorityOrderer",
        ordererAssemblyName: "Api.IntegrationTest2"
    )]
    public class HealthCheckControllerTest
    {
        private readonly HttpClient _client;

        public HealthCheckControllerTest(TestWebApplicationFactory<Program, DatabaseContext> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, TestAuthenticationHandler.TOKEN);
        }

        [Fact, TestPriority(1)]
        public async Task HealthCheck_ShouldBeResponse()
        {
            // Arrange

            // Act
            //var response = await client.GetAsync("/health");
            var response = await _client.GetAsync("/healthcheck");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}