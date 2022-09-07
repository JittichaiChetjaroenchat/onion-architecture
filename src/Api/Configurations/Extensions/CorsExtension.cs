using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configurations.Extensions
{
    public static class CorsExtension
    {
        public static void AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(config =>
            {
                config.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });
        }

        public static void UseCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseCors(config => config
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()
            );
        }
    }
}