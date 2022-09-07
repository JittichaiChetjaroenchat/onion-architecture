using System;
using System.Threading.Tasks;
using Application.Persistence;
using Application.Repositories;
using Application.Repositories.Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace Repository.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseContext _context;
        private bool _disposed;

        private ICustomerRepository _customers { get; set; }
        public ICustomerRepository Customers => _customers ?? (_customers = new CustomerRepository(_context));

        public UnitOfWork(
            IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _context.RollbackAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose context
            }

            _disposed = true;
        }
    }
}