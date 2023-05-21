using System;
using System.Collections.Generic;

namespace Google.Client.Configurations
{
    public class HttpClientConnectorConfiguration
    {
        public string Name { get; set; }

        public string BaseUrl { get; set; }

        public TimeSpan HttpMessageHandlerLifetime { get; set; }

        public int RequestTimeOut { get; set; }     // In milliseconds

        public List<HttpClientConnectorConfigurationSetting> Settings { get; set; }
    }
}