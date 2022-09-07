using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Domain.Models.Token;
using Microsoft.IdentityModel.Tokens;

namespace Service.Services
{
    public class TokenService : ITokenService
    {
        public async Task<TokenInfo> GenerateAsync(string key, string name)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new Claim(ClaimTypes.Name, name)
                    }
                ),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var value = tokenHandler.WriteToken(token);
            var result = new TokenInfo { Value = value, Expires = token.ValidTo };

            return await Task.FromResult(result);
        }
    }
}