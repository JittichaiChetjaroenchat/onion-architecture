using System.Threading.Tasks;
using Application.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Persistence
{
    public class DatabaseContext : IdentityDbContext<User>, IDatabaseContext
    {
        public virtual DbSet<Customer> Customers { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await base.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await base.Database.CommitTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            await base.Database.RollbackTransactionAsync();
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

            base.OnModelCreating(modelBuilder);     // Needed for identity
        }
    }
}