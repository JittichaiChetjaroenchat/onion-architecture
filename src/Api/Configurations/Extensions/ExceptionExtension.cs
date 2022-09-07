using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configurations.Extensions
{
    public static class ExceptionExtension
    {
        public static void AddExceptionConfigure(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;     // No ModelState validation (Use custom validation instead)
            });
        }
    }
}