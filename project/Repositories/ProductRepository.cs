using EfCoreProject.BaseRepository;
using EfCoreProject.Context;
using EfCoreProject.DbFactories;
using EfCoreProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace EfCoreProject.Repositories
{
    public class ProductReadRepository : RepositoryBase<ReadProductDbContext, Product>, IReadProductRepository
    {
        private readonly ReadProductDbContext _readContext;


        public ProductReadRepository(DbFactory<ReadProductDbContext> readContextFactory) : base(readContextFactory)
        {
            _readContext = readContextFactory?.Context;
        }

        public async ValueTask<Product> GetById(int id)
        {
            var result =await _readContext.Set<Product>().Where(x => x.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public override IUnitOfWork GetUnitOfWork()
        {
            //不需要事先
            throw new NotImplementedException();
        }
    }

    public class ProductWriteRepository : RepositoryBase<WriteProductDbContext, Product>, IWriteProductRepository
    {
        private readonly WriteProductDbContext _readContext;


        public ProductWriteRepository(DbFactory<WriteProductDbContext> readContextFactory) : base(readContextFactory)
        {
            _readContext = readContextFactory?.Context;
        }


        public override IUnitOfWork GetUnitOfWork()
        {
            if (_readContext is WriteProductDbContext)
                return (WriteProductDbContext)_readContext;
            else
                throw new ArgumentNullException(nameof(_readContext));
        }
    }
}
