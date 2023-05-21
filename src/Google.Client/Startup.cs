using System;
using System.Net.Http.Headers;
using System.Threading;
using Google.Client.Configurations;
using Google.Client.Constants;
using Google.Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Google.Client
{
    public static class Startup
    {
        public static void AddGoogleClient(this IServiceCollection services, ConfigurationManager configuration)
        {
            // Get configuration
            var httpClientConfiguration = configuration.GetSection(HttpClientConfiguration.SectionName).Get<HttpClientConfiguration>();
            var googleServiceConnectorConfiguration = httpClientConfiguration.Get(HttpClientConnectorConfig.Name);

            // Add dependencies
            services.AddScoped<IGoogleClient, GoogleClient>();

            // Build http client
            var googleHttpClientBuilder = services.AddHttpClient<IGoogleClient, GoogleClient>()
                .SetHandlerLifetime(googleServiceConnectorConfiguration.HttpMessageHandlerLifetime == TimeSpan.Zero ? Timeout.InfiniteTimeSpan : googleServiceConnectorConfiguration.HttpMessageHandlerLifetime)
                .ConfigureHttpClient(httpClient =>
                {
                    httpClient.BaseAddress = new Uri(googleServiceConnectorConfiguration.BaseUrl);
                    httpClient.Timeout = TimeSpan.FromMilliseconds(googleServiceConnectorConfiguration.RequestTimeOut);
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sample token");
                });
        }
    }
}