using System.Threading.Tasks;
using Application.Repositories.Base;

namespace Application.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Domain.Entities.Customer>
    {
        Task<Domain.Entities.Customer> GetByUniqueKeyAsync(string name);
    }
}