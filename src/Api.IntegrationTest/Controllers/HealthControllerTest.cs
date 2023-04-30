using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Api.IntegrationTest.Controllers
{
    [TestFixture]
    public class HealthControllerTest
    {
        private TestWebApplicationFactory _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new TestWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (_client != null)
            {
                _client.Dispose();
            }

            if (_factory != null)
            {
                _factory.Dispose();
            }
        }

        [Test, Order(1)]
        public async Task HealthCheck_ShouldBeResponse()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/health").ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}