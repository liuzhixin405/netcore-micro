using EfCoreProject.Models;
using RepositoryComponent.BaseRepo;

namespace EfCoreProject.Repositories
{
    public interface IReadProductRepository:IReadRepository<Product>
    {
        ValueTask<Product> GetById(int id);
    }

   
}
