using chatgptwriteproject.Models;
using chatgptwriteproject.Repositories;

namespace chatgptwriteproject.Context
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
    }
}
