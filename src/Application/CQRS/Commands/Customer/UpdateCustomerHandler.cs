using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Helpers;
using Application.Services;
using Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Application.CQRS.Commands.Customer
{
    public class UpdateCustomerRequest : IRequest<Response<UpdateCustomerResponse>>
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }
    }

    public class UpdateCustomerResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty("updated_on")]
        public DateTime UpdatedOn { get; set; }
    }

    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerRequest, Response<UpdateCustomerResponse>>
    {
        private readonly ICustomerService _customerService;

        public UpdateCustomerHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Response<UpdateCustomerResponse>> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            // Build model
            var model = new Domain.Models.Customer.UpdateCustomer
            {
                Id = request?.Id ?? Guid.Empty,
                Name = request?.Name,
                Age = request?.Age ?? 0
            };

            // Call service
            var updatedCustomer = await _customerService.UpdateAsync(model);

            // Build result
            var result = new UpdateCustomerResponse
            {
                Id = updatedCustomer.Id,
                Name = updatedCustomer.Name,
                Age = updatedCustomer.Age,
                CreatedOn = updatedCustomer.CreatedOn,
                UpdatedOn = updatedCustomer.UpdatedOn
            };

            return ResponseHelper.Ok(result);
        }
    }
}