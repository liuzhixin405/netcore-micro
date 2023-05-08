using EfCoreProject.Context;
using EfCoreProject.DbFactories;
using EfCoreProject.Models;
using EfCoreProject.Repositories;
using Microsoft.EntityFrameworkCore;
using project.Dtos.Product;

namespace EfCoreProject.Services
{
    public class ProductService : IProductService
    {
        private readonly IWriteProductRepository _writeProductRepository;
        private readonly IReadProductRepository _readProductRepository;
        private readonly IConfiguration _configuration;

        public ProductService(IWriteProductRepository writeProductRepository
            , IReadProductRepository readProductRepository
            ,IConfiguration configuration)
        {
            _writeProductRepository = writeProductRepository;
            _readProductRepository = readProductRepository;
            _configuration = configuration;
        }
        public async Task Add(CreateProductDto product)
        {
            var newProduct = new Product
            {
                Name = product.Name,
                Price = product.Price,
            };
           
            _writeProductRepository.Add(newProduct);
            await _writeProductRepository.GetUnitOfWork().SaveChangeAsync();
        }

        public async Task Delete(Product entity)
        {
            _writeProductRepository.Delete(entity);
            await _writeProductRepository.GetUnitOfWork().SaveChangeAsync();
        }

        public Task<IEnumerable<Product>> GetList()
        {
            return _readProductRepository.GetList();
        }

        public ValueTask<Product> GetById(int id)
        {
            return _readProductRepository.GetById(id);
        }

        public async Task Update(Product entity)
        {
            _writeProductRepository.Update(entity);
            await _writeProductRepository.GetUnitOfWork().SaveChangeAsync();
        }
    }
}
