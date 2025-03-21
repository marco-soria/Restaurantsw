// Restaurants.Domain/Services/ITransactionService.cs
using System.Threading;
using System.Threading.Tasks;

namespace Restaurants.Domain.Services
{
    public interface ITransactionService
    {
        Task<ITransactionScope> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }

    public interface ITransactionScope : IAsyncDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}