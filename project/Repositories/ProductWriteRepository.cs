using EfCoreProject.Context;
using EfCoreProject.Models;
using EfCoreProject.Repositories;
using project.Repositories;
using RepositoryComponent.BaseRepo;
using RepositoryComponent.DbFactories;

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
