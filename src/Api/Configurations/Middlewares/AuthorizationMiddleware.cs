using System.Net;
using System.Threading.Tasks;
using Application.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Api.Configurations.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public AuthorizationMiddleware(
            RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            await _requestDelegate.Invoke(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                var errors = ResponseHelper.Build("Unauthorized access");
                var result = ResponseHelper.Error<object>(errors);
                var text = JsonConvert.SerializeObject(result);

                await context.Response.WriteAsync(text);
            }
        }
    }
}