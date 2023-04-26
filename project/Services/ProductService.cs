using chatgptwriteproject.Context;
using chatgptwriteproject.Models;
using chatgptwriteproject.Repositories;

namespace chatgptwriteproject.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task Add(Product product)
        {
            _repository.Add(product);
            await _repository.GetUnitOfWork().SaveChangeAsync();
        }

        public async Task Delete(Product entity)
        {
            _repository.Delete(entity);
            await _repository.GetUnitOfWork().SaveChangeAsync();
        }

        public Task<IEnumerable<Product>> GetList()
        {
            return _repository.GetList();
        }

        public ValueTask<Product> GetById(int id)
        {
            return _repository.GetById(id);
        }

        public async Task Update(Product entity)
        {
            _repository.Update(entity);
            await _repository.GetUnitOfWork().SaveChangeAsync();
        }
    }
}
