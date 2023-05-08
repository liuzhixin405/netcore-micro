using EfCoreProject.Models;
using project.Dtos.Product;

namespace EfCoreProject.Services
{
    public interface IProductService
    {
        Task Add(CreateProductDto product);
        Task<IEnumerable<Product>> GetList();
        ValueTask<Product> GetById(int id);
        Task Update(Product entity);
        Task Delete(Product entity);
    }
}
