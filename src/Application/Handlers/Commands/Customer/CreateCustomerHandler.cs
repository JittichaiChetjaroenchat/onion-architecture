using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Helpers;
using Application.Services;
using Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Application.Handlers.Commands.Customer
{
    public class CreateCustomerRequest : IRequest<Response<CreateCustomerResponse>>
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }
    }

    public class CreateCustomerResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    public class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest, Response<CreateCustomerResponse>>
    {
        private readonly ICustomerService _customerService;

        public CreateCustomerHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Response<CreateCustomerResponse>> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            // Build model
            var model = new Domain.Models.Customer.CreateCustomer
            {
                Name = request?.Name,
                Age = request?.Age ?? 0
            };

            // Call service
            var customerId = await _customerService.CreateAsync(model);

            // Build result
            var result = new CreateCustomerResponse { Id = customerId };

            return ResponseHelper.Ok(result);
        }
    }
}