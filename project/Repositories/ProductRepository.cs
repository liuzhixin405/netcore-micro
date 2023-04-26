using chatgptwriteproject.BaseRepository;
using chatgptwriteproject.Context;
using chatgptwriteproject.DbFactories;
using chatgptwriteproject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace chatgptwriteproject.Repositories
{
    public class ProductRepository : RepositoryBase<ReadProductDbContext, WriteProductDbContext, Product>, IProductRepository
    {
        private readonly ReadProductDbContext _readContext;
        private readonly WriteProductDbContext _writeContext;

        public ProductRepository(DbFactory<ReadProductDbContext, WriteProductDbContext> dbfactory) : base(dbfactory)
        {
            _readContext = dbfactory.Context.Item1;
            _writeContext = dbfactory.Context.Item2;
        }

        public async ValueTask<Product> GetById(int id)
        {
            var result =await _readContext.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public override IUnitOfWork GetUnitOfWork()
        {
            return _writeContext;
        }
    }

}
