using EfCoreProject.BaseRepo;
using EfCoreProject.Context;
using EfCoreProject.Models;

namespace EfCoreProject.Repositories
{
    public interface IReadProductRepository:IReadRepository<Product>
    {
        ValueTask<Product> GetById(int id);
    }

   
}
