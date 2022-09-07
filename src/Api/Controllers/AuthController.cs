using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.CQRS.Commands.Auth;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : CustomControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<RegisterResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SigninAsync([FromBody] RegisterRequest payload)
        {
            var result = await _mediator.Send(payload);

            return Ok(result);
        }

        [HttpPost("signin")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<SignInResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SigninAsync([FromBody] SignInRequest payload)
        {
            var result = await _mediator.Send(payload);

            return Ok(result);
        }
    }
}