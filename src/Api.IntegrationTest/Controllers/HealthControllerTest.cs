using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
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
            _client.Dispose();
            _factory.Dispose();
        }

        [Test, Order(1)]
        public async Task CreateCustomer_ShouldBeResponse()
        {
            var response = await _client.GetAsync("/health").ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}