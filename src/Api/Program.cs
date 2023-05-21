using System;
using Api.Configurations.Extensions;
using Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Repository;
using Service;

namespace Api
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add logging (Serilog)
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var applicationName = builder.Configuration.GetValue<string>("ApplicationName");
                var elasticsearchUri = builder.Configuration.GetValue<string>("Elasticsearch:Uri");

                builder.Host.AddSerilogLogging(environment, applicationName, elasticsearchUri);

                // Add CORS
                builder.Services.AddCors(builder.Configuration);

                // Add controllers
                var mvcBuilder = builder.Services.AddControllers(options =>
                {
                    options.RespectBrowserAcceptHeader = true;
                    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                });

                // Add authentication (JWT)
                var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key");
                var jwtIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
                var jwtAudience = builder.Configuration.GetValue<string>("Jwt:Audience");

                builder.Services.AddJwtAuthentication(jwtKey, jwtIssuer, jwtAudience);

                // Add formatting
                mvcBuilder.AddJsonFormat();
                mvcBuilder.AddXmlFormat();

                // Add exception configure
                builder.Services.AddExceptionConfigure();

                // Add client's rate limit
                builder.Services.AddClientRateLimit(builder.Configuration);

                // Add localize
                builder.Services.AddLocalize(builder.Configuration);

                // Add health checks
                builder.Services.AddHealthChecks(builder.Configuration);

                // Add routing
                builder.Services.AddRouting(options => options.LowercaseUrls = true);

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddSwaggerDocumentation("Demo API", "v1");

                // Add layers
                builder.Services.AddApplication();
                builder.Services.AddService();
                builder.Services.AddRepository();
                builder.Services.AddPersistence(builder.Configuration);

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwaggerDocumention();
                }

                app.UseHttpsRedirection();

                app.UseRouting();

                // Use authorize
                app.UseAuthorize();

                app.UseAuthentication();
                app.UseAuthorization();

                // Use client's rate limit
                app.UseClientRateLimit();

                // Use localize
                app.UseLocalize();

                // Use CORS
                app.UseCors(builder.Configuration);

                // User database
                app.UsePersistence();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();

                    // Use health checks
                    endpoints.UseHealthChecks(builder.Configuration);
                });

                app.Run();
            }
            catch (Exception ex)
            {

            }
        }
    }
}