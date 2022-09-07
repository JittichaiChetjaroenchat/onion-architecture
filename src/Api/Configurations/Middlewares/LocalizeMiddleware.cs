using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Api.Configurations.Middlewares
{
    public class LocalizeMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly IConfiguration _configuration;

        public LocalizeMiddleware(
            RequestDelegate requestDelegate,
            IConfiguration configuration)
        {
            _requestDelegate = requestDelegate;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var acceptLanguage = context.Request.GetAcceptLanguage();
            if (!string.IsNullOrEmpty(acceptLanguage))
            {
                var defaultCulture = _configuration.GetValue<string>("Culture:Default");
                var supportedCultures = _configuration.GetSection("Culture:Supports").Get<string[]>();
                var culture = supportedCultures.Contains(acceptLanguage) ? acceptLanguage : defaultCulture;

                try
                {
                    var cultureInfo = CultureInfo.GetCultureInfo(culture);
                    ErrorMessage.Culture = cultureInfo;
                }
                catch { }
            }

            await _requestDelegate.Invoke(context);
        }
    }

    public static class HttpRequestExtension
    {
        public static string GetAcceptLanguage(this HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }

            string language = null;

            // Get from standard header
            language = request.Headers[HttpRequestHeader.AcceptLanguage.ToStandardName()].ToString();
            if (!string.IsNullOrEmpty(language))
            {
                return language;
            }

            // Get from custom header
            language = request.Headers[$"X-{HttpRequestHeader.AcceptLanguage.ToStandardName()}"].ToString();
            if (!string.IsNullOrEmpty(language))
            {
                return language;
            }

            return null;
        }

        public static string ToStandardName(this HttpRequestHeader requestHeader)
        {
            string headerName = requestHeader.ToString();

            var headerStandardNameBuilder = new StringBuilder();
            headerStandardNameBuilder.Append(headerName[0]);

            for (int index = 1; index < headerName.Length; index++)
            {
                char character = headerName[index];
                if (char.IsUpper(character))
                {
                    headerStandardNameBuilder.Append('-');
                }

                headerStandardNameBuilder.Append(character);
            }

            string headerStandardName = headerStandardNameBuilder.ToString();

            return headerStandardName;
        }
    }
}