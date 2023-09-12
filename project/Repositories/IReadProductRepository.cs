using project.Models;
using RepositoryComponent.BaseRepo;

namespace project.Repositories
{
    public interface IReadProductRepository:IReadRepository<Product>
    {
        ValueTask<Product> GetById(string id);
    }

   
}
