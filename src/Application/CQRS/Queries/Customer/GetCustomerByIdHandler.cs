using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Helpers;
using Application.Services;
using Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Application.CQRS.Queries.Customer
{
    public class GetCustomerByIdRequest : IRequest<Response<GetCustomerByIdResponse>>
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    public class GetCustomerByIdResponse
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

    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdRequest, Response<GetCustomerByIdResponse>>
    {
        private readonly ICustomerService _customerService;

        public GetCustomerByIdHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Response<GetCustomerByIdResponse>> Handle(GetCustomerByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Call service
                var customer = await _customerService.GetByIdAsync(request.Id);

                // Build result
                var result = new GetCustomerByIdResponse { 
                    Id = customer.Id, 
                    Name = customer.Name, 
                    Age = customer.Age,
                    CreatedOn = customer.CreatedOn, 
                    UpdatedOn = customer.UpdatedOn
                };

                return ResponseHelper.Ok(result);
            }
            catch (Exception ex)
            {
                var errors = new string[] { ex.Message };

                return ResponseHelper.Error<GetCustomerByIdResponse>(errors);
            }
        }
    }
}