using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Api.Configurations.Extensions
{
    public static class RateLimitExtension
    {
        public static void AddIpRateLimitInMemory(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddInMemoryRateLimiting();

            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        }

        public static void AddIpRateLimitDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = ConfigurationOptions.Parse(redisConnectionString);
            });
            services.AddDistributedRateLimiting();

            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        }

        public static void AddClientRateLimitInMemory(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddInMemoryRateLimiting();

            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.Configure<ClientRateLimitOptions>(configuration.GetSection("ClientRateLimiting"));
        }

        public static void AddClientRateLimitDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = ConfigurationOptions.Parse(redisConnectionString);
            });
            services.AddDistributedRateLimiting();

            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.Configure<ClientRateLimitOptions>(configuration.GetSection("ClientRateLimiting"));
        }

        public static void UseIpRateLimit(this IApplicationBuilder app)
        {
            app.UseMiddleware<IpRateLimitMiddleware>();
        }

        public static void UseClientRateLimit(this IApplicationBuilder app)
        {
            app.UseMiddleware<ClientRateLimitMiddleware>();
        }
    }
}