using EfCoreProject.BaseRepo;
using EfCoreProject.Context;
using EfCoreProject.DbFactories;
using EfCoreProject.Models;
using EfCoreProject.Repositories;

namespace EfCoreProject.Repositories
{
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
