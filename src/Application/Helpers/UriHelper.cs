using Microsoft.AspNetCore.Http;

namespace Application.Helpers
{
    public static class UriHelper
    {
        public static string BuildLocation(HttpRequest request, object value)
        {
            return $"{request.Scheme}://{request.Host.Value}{request.Path}/{value}";
        }
    }
}