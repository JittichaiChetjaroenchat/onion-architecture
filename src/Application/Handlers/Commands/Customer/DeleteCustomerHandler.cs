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
    public class DeleteCustomerRequest : IRequest<Response<DeleteCustomerResponse>>
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    public class DeleteCustomerResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerRequest, Response<DeleteCustomerResponse>>
    {
        private readonly ICustomerService _customerService;

        public DeleteCustomerHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Response<DeleteCustomerResponse>> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
        {
            // Call service
            var isSuccess = await _customerService.DeleteAsync(request.Id);

            // Build result
            var result = new DeleteCustomerResponse { Success = isSuccess };

            return ResponseHelper.Ok(result);
        }
    }
}