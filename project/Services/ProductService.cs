using project.Models;
using project.Repositories;
using Microsoft.EntityFrameworkCore;
using project.Dtos;
using project.Repositories;
using System.Linq.Expressions;
using LinqKit;
using project.Utility.Helper;
using RepositoryComponent.Page;
using AutoMapper;
using DistributedId;

namespace project.Services
{
    public class ProductService : IProductService
    {
        private readonly IWriteProductRepository _writeProductRepository;
        private readonly IReadProductRepository _readProductRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedId _distributedId;
        public ProductService(IWriteProductRepository writeProductRepository
            , IReadProductRepository readProductRepository
            ,IMapper mapper
            ,IDistributedId distributedId)
        {
            _writeProductRepository = writeProductRepository;
            _readProductRepository = readProductRepository;
            _mapper = mapper;
            _distributedId = distributedId;
        }
        public async Task<bool> Add(CreateProductDto product)
        {
            var newProduct 
             //= new Product
            //{
            //    Name = product.Name,
            //    Price = product.Price,
            //    CreateTime = TimestampHelper.ToUnixTimeMilliseconds(DateTime.UtcNow)
            //};
            =
            _mapper.Map<Product>(product);
            newProduct.Id = _distributedId.NewLongId();
            await _writeProductRepository.AddAsync(newProduct);
            var result = await _writeProductRepository.SaveChangeAsync();
            return result == 1;
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

        public ValueTask<Product> GetById(long id)
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
