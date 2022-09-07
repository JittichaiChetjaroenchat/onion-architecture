using System;
using System.Threading.Tasks;
using Application.Repositories.Base;
using Application.Resources;
using Application.Services;
using Application.Validations.Customers;
using Domain.Models.Customer;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(
            ILogger<CustomerService> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetCustomer> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Begin:GetByIdAsync with id: {id}");

            // Query
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);

            // Validate
            if (customer == null)
            {
                throw new Exception(ErrorMessage._00001);
            }

            // Build result
            var result = new GetCustomer
            {
                Id = customer.Id,
                Name = customer.Name,
                Age = customer.Age,
                CreatedOn = customer.CreatedOn,
                UpdatedOn = customer.UpdatedOn
            };

            _logger.LogInformation($"End:GetByIdAsync with id: {id}");

            return result;
        }

        public async Task<Guid> CreateAsync(CreateCustomer data)
        {
            // Validate
            await new CreateCustomerValidation().ValidateAndThrowAsync(data);

            // Check exists
            var persistence = await _unitOfWork.Customers.GetByUniqueKeyAsync(data.Name);
            if (persistence != null)
            {
                throw new Exception(ErrorMessage._00002);
            }

            // Build entity
            var customer = new Domain.Entities.Customer
            {
                Id = Guid.NewGuid(),
                Name = data.Name,
                Age = data.Age
            };

            // Call Unit of Work
            var isSuccess = await _unitOfWork.Customers.AddAsync(customer);
            if (!isSuccess)
            {
                throw new Exception(ErrorMessage._99999);
            }

            return customer.Id;
        }

        public async Task<GetCustomer> UpdateAsync(UpdateCustomer data)
        {
            // Vallidate
            var persistence = await _unitOfWork.Customers.GetByIdAsync(data.Id);
            if (persistence == null)
            {
                throw new Exception(ErrorMessage._00001);
            }

            var newName = await _unitOfWork.Customers.GetByUniqueKeyAsync(data.Name);
            if (newName != null)
            {
                throw new Exception(ErrorMessage._00002);
            }

            // Build entity
            persistence.Name = data.Name;
            persistence.Age = data.Age;
            persistence.UpdatedOn = DateTime.Now;

            // Call unit of work
            var isSuccess = await _unitOfWork.Customers.UpdateAsync(persistence);
            if (!isSuccess)
            {
                throw new Exception(ErrorMessage._99999);
            }

            // Build result
            var result = new GetCustomer
            {
                Id = persistence.Id,
                Name = persistence.Name,
                Age = persistence.Age,
                CreatedOn = persistence.CreatedOn,
                UpdatedOn = persistence.UpdatedOn
            };

            return result;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            // Validate
            var persistence = await _unitOfWork.Customers.GetByIdAsync(id);
            if (persistence == null)
            {
                throw new Exception(ErrorMessage._00001);
            }

            // Call unit of work
            var isSuccess = await _unitOfWork.Customers.RemoveAsync(persistence);
            if (!isSuccess)
            {
                throw new Exception(ErrorMessage._99999);
            }

            return isSuccess;
        }
    }
}