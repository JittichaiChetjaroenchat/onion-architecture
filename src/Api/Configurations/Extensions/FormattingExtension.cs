using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Api.Configurations.Extensions
{
    public static class FormattingExtension
    {
        public static void AddJsonFormat(this IMvcBuilder mvc)
        {
            mvc.AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.Formatting = Formatting.Indented;

                options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                options.SerializerSettings.FloatParseHandling = FloatParseHandling.Decimal;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
                options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Error;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                // Configure a custom converter
                options.SerializerSettings.Converters.Add(new StringEnumConverter());

                // Culture
                options.SerializerSettings.Culture = CultureInfo.InvariantCulture;
            });
        }

        public static void AddXmlFormat(this IMvcBuilder mvc)
        {
            mvc.AddXmlDataContractSerializerFormatters();
        }
    }
}