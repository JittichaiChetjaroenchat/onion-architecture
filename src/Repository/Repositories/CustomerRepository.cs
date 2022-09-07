using System.Threading.Tasks;
using Application.Persistence;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Base;

namespace Repository.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDatabaseContext context) 
            : base(context)
        {
        }

        public async Task<Customer> GetByUniqueKeyAsync(string name)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }
    }
}