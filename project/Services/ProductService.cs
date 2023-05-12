using EfCoreProject.Context;
using EfCoreProject.DbFactories;
using EfCoreProject.Models;
using EfCoreProject.Repositories;
using Microsoft.EntityFrameworkCore;
using project.Dtos.Product;
using project.Page;
using project.Repositories;
using static Dapper.SqlMapper;
using System.Linq.Expressions;
using LinqKit;
using project.Utility.Helper;

namespace EfCoreProject.Services
{
    public class ProductService : IProductService
    {
        private readonly IWriteProductRepository _writeProductRepository;
        private readonly IReadProductRepository _readProductRepository;
        private readonly IConfiguration _configuration;

        public ProductService(IWriteProductRepository writeProductRepository
            , IReadProductRepository readProductRepository
            , IConfiguration configuration)
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
                CreateTime = TimestampHelper.ToUnixTimeMilliseconds(DateTime.UtcNow)
            };

            await _writeProductRepository.AddAsync(newProduct);
            await _writeProductRepository.SaveChangeAsync();
        }

        public async Task Delete(Product entity)
        {
            await _writeProductRepository.RemoveAsync(entity);
            await _writeProductRepository.SaveChangeAsync();
        }

        public Task<IEnumerable<Product>> GetList()
        {
            return _readProductRepository.GetListAsync();
        }

        public ValueTask<Product> GetById(int id)
        {
            return _readProductRepository.GetById(id);
        }

        public async Task Update(Product entity)
        {
            await _writeProductRepository.UpdateAsync(entity);
            await _writeProductRepository.SaveChangeAsync();
        }
        /*
           {
              "search": {
                "name": "cdx"
              }
            }
         */
        public async Task<PaginatedList<Product>> PageList(PaginatedOptions<PageProductDto> query)
        {
            Expression<Func<Product, bool>> predicate = x => true;
            var hasSearch = false;
            if (!string.IsNullOrEmpty(query.Search.Name))
            {
                predicate = predicate.And(x => x.Name.Contains(query.Search.Name));
                hasSearch = true;
            }
            if (query.Search.Price.HasValue)
            {
                predicate = predicate.And(x => x.Price == query.Search.Price);
                hasSearch = true;
            }
            return hasSearch == true ? await _readProductRepository.GetPaginatedListAsync(predicate, query) :
                 await _readProductRepository.GetPaginatedListAsync(query);
        }
    }
}
