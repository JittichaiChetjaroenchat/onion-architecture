using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Persistence;
using Xunit;

namespace Api.IntegrationTest2
{
    public class IntegrationTestBase : IClassFixture<TestWebApplicationFactory<Program, DatabaseContext>>
    {
        public readonly TestWebApplicationFactory<Program, DatabaseContext> Factory;
        public readonly HttpClient Client;
        public readonly DatabaseContext DbContext;

        public IntegrationTestBase(TestWebApplicationFactory<Program, DatabaseContext> factory)
        {
            Factory = factory;
            Client = Factory.CreateClient();
            Client.DefaultRequestHeaders.Add(HeaderNames.Authorization, TestAuthenticationHandler.TOKEN);

            var scope = factory.Services.CreateScope();
            DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        }
    }
}