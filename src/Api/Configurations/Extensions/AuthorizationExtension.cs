using Api.Configurations.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Api.Configurations.Extensions
{
    public static class AuthorizationExtension
    {
        public static void UseAuthorize(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}