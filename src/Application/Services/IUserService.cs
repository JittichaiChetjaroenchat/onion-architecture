using System.Threading.Tasks;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public interface IUserService
    {
        Task<bool> ValidateAsync(ValidateUser data);

        Task<IdentityResult> CreateAsync(CreateUser data);
    }
}