// Restaurants.Infrastructure/Services/TransactionService.cs
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Restaurants.Domain.Services;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Services
{
    internal class TransactionService : ITransactionService
    {
        private readonly RestaurantsDbContext _dbContext;

        public TransactionService(RestaurantsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ITransactionScope> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            return new TransactionScope(transaction);
        }

        private class TransactionScope : ITransactionScope
        {
            private readonly IDbContextTransaction _transaction;

            public TransactionScope(IDbContextTransaction transaction)
            {
                _transaction = transaction;
            }

            public async Task CommitAsync(CancellationToken cancellationToken = default)
            {
                await _transaction.CommitAsync(cancellationToken);
            }

            public async Task RollbackAsync(CancellationToken cancellationToken = default)
            {
                await _transaction.RollbackAsync(cancellationToken);
            }

            public async ValueTask DisposeAsync()
            {
                await _transaction.DisposeAsync();
            }
        }
    }
}