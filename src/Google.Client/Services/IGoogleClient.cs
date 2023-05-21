using System.Net.Http;
using System.Threading.Tasks;

namespace Google.Client.Services
{
    public interface IGoogleClient
    {
        Task<HttpResponseMessage> PingAsync();
    }
}