using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Api.Controllers.Base;
using Application.Handlers.Commands.Customer;
using Application.Handlers.Queries.Customer;
using Application.Helpers;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : CustomControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<GetCustomerByIdResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var payload = new GetCustomerByIdRequest { Id = id };
            var result = await _mediator.Send(payload);

            return Ok(result);
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<CreateCustomerResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerRequest payload)
        {
            var result = await _mediator.Send(payload);
            var uri = UriHelper.BuildLocation(HttpContext.Request, result.Data.Id);

            return Created(uri, result);
        }

        [HttpPut("{id:guid}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<UpdateCustomerResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateCustomerRequest payload)
        {
            payload.Id = id;
            var result = await _mediator.Send(payload);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<DeleteCustomerResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var payload = new DeleteCustomerRequest { Id = id };
            var result = await _mediator.Send(payload);

            return Ok(result);
        }
    }
}