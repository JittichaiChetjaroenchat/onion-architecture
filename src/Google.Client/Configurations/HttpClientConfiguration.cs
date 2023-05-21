using System.Collections.Generic;
using System.Linq;

namespace Google.Client.Configurations
{
    public class HttpClientConfiguration
    {
        public const string SectionName = "HttpClients";

        public List<HttpClientConnectorConfiguration> Connectors { get; set; }

        public HttpClientConnectorConfiguration Get(string name)
        {
            return Connectors.FirstOrDefault(x => x.Name == name);
        }
    }
}