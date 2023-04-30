using Api.IntegrationTest2.Attributes;
using Persistence;
using Xunit;

namespace Api.IntegrationTest2.Controllers
{
    [TestCaseOrderer("Api.IntegrationTest2.Attributes.PriorityOrderer", "Api.IntegrationTest2")]
    public class HealthCheckControllerTest : IntegrationTestBase
    {
        public HealthCheckControllerTest(TestWebApplicationFactory<Program, DatabaseContext> factory) 
            : base(factory)
        {
        }

        [Fact, TestPriority(1)]
        public async Task HealthCheck_ShouldBeResponse()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/health");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}