using EfCoreProject.BaseRepository;
using EfCoreProject.Context;
using EfCoreProject.Models;

namespace EfCoreProject.Repositories
{
    public interface IReadProductRepository:IReadRepository<Product>
    {
        ValueTask<Product> GetById(int id);
    }

    public interface IWriteProductRepository:IWriteRepository<Product>
    { 

    }  
}
