using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories.Base
{
    public interface IGenericRepository<T> where T :class
    {
        Task<T> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> RemoveAsync(T entity);
    }
}