using System;
using System.Threading.Tasks;
using Domain.Models.Customer;

namespace Application.Services
{
    public interface ICustomerService
    {
        Task<GetCustomer> GetByIdAsync(Guid id);

        Task<Guid> CreateAsync(CreateCustomer data);

        Task<GetCustomer> UpdateAsync(UpdateCustomer data);

        Task<bool> DeleteAsync(Guid id);
    }
}