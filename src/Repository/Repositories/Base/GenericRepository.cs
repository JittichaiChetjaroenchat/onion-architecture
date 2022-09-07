using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Persistence;
using Application.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories.Base
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected IDatabaseContext _context;
        protected DbSet<T> _dbSet;

        public GenericRepository(IDatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<bool> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            var affectedRow = await _context.SaveChangesAsync();

            return affectedRow > 0 ? true : false;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            var affectedRow = await _context.SaveChangesAsync();

            return affectedRow > 0 ? true : false;
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            var affectedRow = await _context.SaveChangesAsync();

            return affectedRow > 0 ? true : false;
        }
    }
}