using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Helpers;
using Application.Services;
using Domain.Models;
using Domain.Models.User;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Application.CQRS.Commands.Auth
{
    public class SignInRequest : IRequest<Response<SignInResponse>>
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class SignInResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expires")]
        public DateTime Expires { get; set; }
    }

    public class SigninHandler : IRequestHandler<SignInRequest, Response<SignInResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public SigninHandler(
            IConfiguration configuration,
            ITokenService tokenService,
            IUserService userService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task<Response<SignInResponse>> Handle(SignInRequest request, CancellationToken cancellationToken)
        {
            // Call service
            var payload = new ValidateUser
            {
                Email = request.Email,
                Password = request.Password
            };
            var isValid = await _userService.ValidateAsync(payload);

            // Check valid
            if (!isValid)
            {
                throw new Exception("Invalid email or password.");
            }

            // Genereate Token
            var jwtKey = _configuration.GetValue<string>("Jwt:Key");
            var token = await _tokenService.GenerateAsync(jwtKey, request.Email);

            // Build result
            var result = new SignInResponse
            {
                Token = token.Value,
                Expires = token.Expires
            };

            return ResponseHelper.Ok(result);
        }
    }
}