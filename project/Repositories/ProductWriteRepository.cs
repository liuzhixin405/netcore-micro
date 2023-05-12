using EfCoreProject.BaseRepo;
using EfCoreProject.Context;
using EfCoreProject.DbFactories;
using EfCoreProject.Models;
using EfCoreProject.Repositories;
using project.Repositories;

namespace EfCoreProject.Repositories
{
    public class ProductWriteRepository : WriteRepository<WriteProductDbContext, Product>, IWriteProductRepository
    {
        private readonly WriteProductDbContext _readContext;


        public ProductWriteRepository(DbFactory<WriteProductDbContext> readContextFactory) : base(readContextFactory)
        {
            _readContext = readContextFactory?.Context;
        }
    }
}
