using Api.Configurations.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configurations.Extensions
{
    public static class LocalizeExtension
    {
        public static void AddLocalize(this IServiceCollection services, IConfiguration configuration)
        {
            var defaultCulture = configuration.GetValue<string>("Culture:Default");
            var supportedCultures = configuration.GetSection("Culture:Supports").Get<string[]>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture(defaultCulture);
                options.AddSupportedCultures(supportedCultures);
                options.FallBackToParentCultures = true;
            });
        }

        public static void UseLocalize(this IApplicationBuilder app)
        {
            app.UseMiddleware<LocalizeMiddleware>();
        }
    }
}