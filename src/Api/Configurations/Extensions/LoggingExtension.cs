using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Api.Configurations.Extensions
{
    public static class LoggingExtension
    {
        public static void AddSerilogLogging(this ConfigureHostBuilder host, string environment, string applicationName, string elasticsearchUri)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.json.{environment}.json", optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithMachineName()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(environment, applicationName, elasticsearchUri))
                .Enrich.WithProperty("ENV", environment)
                .Enrich.WithProperty("APP_NAME", applicationName)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            host.UseSerilog();
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(string environment, string applicationName, string elasticsearchUri)
        {
            return new ElasticsearchSinkOptions(new Uri(elasticsearchUri))
            {
                IndexFormat = $"{applicationName.ToLower()}-logs-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
                AutoRegisterTemplate = true,
                NumberOfShards = 2,
                NumberOfReplicas = 1
            };
        }
    }
}