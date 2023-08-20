using project.Models;
using project.Dtos;
using RepositoryComponent.Page;

namespace project.Services
{
    public interface IProductService
    {
        Task Add(CreateProductDto product);
        Task<IEnumerable<Product>> GetList();
        ValueTask<Product> GetById(int id);
        Task Update(Product entity);
        Task Delete(Product entity);
        Task<PaginatedList<Product>> PageList(PaginatedOptions<PageProductDto> query);
    }
}
