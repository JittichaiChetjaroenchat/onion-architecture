using System.Net.Http;
using System.Threading.Tasks;
using Google.Client.Constants;

namespace Google.Client.Services
{
    public class GoogleClient : IGoogleClient
    {
        private readonly HttpClient _httpClient;

        public GoogleClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> PingAsync()
        {
            return await _httpClient.GetAsync(HttpClientConnectorConfig.Uri.Home);
        }
    }
}