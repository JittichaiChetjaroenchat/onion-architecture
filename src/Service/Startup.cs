using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Service.Services;

namespace Service
{
    public static class Startup
    {
        public static void AddService(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICustomerService, CustomerService>();
        }
    }
}