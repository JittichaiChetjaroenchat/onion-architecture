using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configurations.Extensions
{
    public static class HealthCheckExtension
    {
        public static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks();
        }

        public static void UseHealthChecks(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
        {
            endpoints.MapHealthChecks("/health");
        }
    }
}