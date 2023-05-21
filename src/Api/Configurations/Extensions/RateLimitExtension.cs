using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configurations.Extensions
{
    public static class RateLimitExtension
    {
        public static void AddIpRateLimit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();
        }

        public static void AddClientRateLimit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            services.Configure<ClientRateLimitOptions>(configuration.GetSection("ClientRateLimiting"));
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();
        }

        public static void UseClientRateLimit(this IApplicationBuilder app)
        {
            app.UseMiddleware<ClientRateLimitMiddleware>();
        }
    }
}