using System.Threading.Tasks;
using Domain.Models.Token;

namespace Application.Services
{
    public interface ITokenService
    {
        Task<TokenInfo> GenerateAsync(string key, string name);
    }
}