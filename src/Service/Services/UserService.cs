using System.Threading.Tasks;
using Application.Services;
using Domain.Entities;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> ValidateAsync(ValidateUser data)
        {
            var user = await _userManager.FindByEmailAsync(data.Email);
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, data.Password);
            var result = user != null && isPasswordValid;

            return await Task.FromResult(result);
        }

        public async Task<IdentityResult> CreateAsync(CreateUser data)
        {
            var entity = new User
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                UserName = data.UserName,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber
            };
            var result = await _userManager.CreateAsync(entity, data.Password);

            return result;
        }
    }
}