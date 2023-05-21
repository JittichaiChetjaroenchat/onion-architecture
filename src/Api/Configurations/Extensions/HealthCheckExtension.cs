using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Client.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Persistence;

namespace Api.Configurations.Extensions
{
    public static class HealthCheckExtension
    {
        public static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddCheck<HealthCheck>(nameof(HealthCheck))
                .AddCheck<DatabaseHealthCheck>(nameof(DatabaseHealthCheck))
                .AddCheck<ApiHealthCheck>(nameof(ApiHealthCheck));
            services
                .AddHealthChecksUI(options =>
                {
                    options.AddHealthCheckEndpoint("Healthcheck API", "/healthcheck");
                })
                .AddInMemoryStorage();
        }

        public static void UseHealthChecks(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
        {
            endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            endpoints.MapHealthChecksUI(options => options.UIPath = "/dashboard");
        }
    }

    public class HealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("The service is up and running."));
            }
            catch (Exception)
            {
                return Task.FromResult(
                    new HealthCheckResult(
                        context.Registration.FailureStatus, "The service is down."));
            }
        }
    }

    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseHealthCheck(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var dbContext = _serviceProvider.GetRequiredService<DatabaseContext>();

                var canConnect = dbContext.Database.CanConnect();
                if (canConnect)
                {
                    return Task.FromResult(HealthCheckResult.Healthy("The database is up and running."));
                }

                return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, "The database is down."));
            }
            catch (Exception)
            {
                return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, "The database is down."));
            }
        }
    }

    public class ApiHealthCheck : IHealthCheck
    {
        private readonly IGoogleClient _googleClient;

        public ApiHealthCheck(IGoogleClient googleClient)
        {
            _googleClient = googleClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var response = await _googleClient.PingAsync();
            if (response.IsSuccessStatusCode)
            {
                return await Task.FromResult(new HealthCheckResult(status: HealthStatus.Healthy, description: "The API is up and running."));
            }

            return await Task.FromResult(new HealthCheckResult(status: HealthStatus.Unhealthy, description: "The API is down."));
        }
    }
}