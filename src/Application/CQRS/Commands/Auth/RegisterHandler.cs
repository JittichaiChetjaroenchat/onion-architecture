using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Helpers;
using Application.Services;
using Domain.Models;
using Domain.Models.User;
using MediatR;
using Newtonsoft.Json;

namespace Application.CQRS.Commands.Auth
{
    public class RegisterRequest : IRequest<Response<RegisterResponse>>
    {
        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }

    public class RegisterResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public class RegisterHandler : IRequestHandler<RegisterRequest, Response<RegisterResponse>>
    {
        private readonly IUserService _userService;

        public RegisterHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Response<RegisterResponse>> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            // Call service
            var payload = new CreateUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Password = request.Password,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };
            var created = await _userService.CreateAsync(payload);

            // Check success
            if (!created.Succeeded)
            {
                var errors = created.Errors
                    .Select(x => x.Description)
                    .ToArray();
                throw new ErrorsException(errors);
            }

            // Build result
            var result = new RegisterResponse
            {
                Success = created.Succeeded
            };

            return ResponseHelper.Ok(result);
        }
    }
}