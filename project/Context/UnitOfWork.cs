using EfCoreProject.Models;
using EfCoreProject.Repositories;

namespace EfCoreProject.Context
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
    }
}
