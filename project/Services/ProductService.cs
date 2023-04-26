using EfCoreProject.Context;
using EfCoreProject.DbFactories;
using EfCoreProject.Models;
using EfCoreProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EfCoreProject.Services
{
    public class ProductService : IProductService
    {
        private readonly IWriteProductRepository _writeProductRepository;
        private readonly IReadProductRepository _readProductRepository;

        public ProductService(IWriteProductRepository writeProductRepository, IReadProductRepository readProductRepository)
        {
            _writeProductRepository = writeProductRepository;
            _readProductRepository = readProductRepository;
        }
        public async Task Add(Product product)
        {
            _writeProductRepository.Add(product);
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
