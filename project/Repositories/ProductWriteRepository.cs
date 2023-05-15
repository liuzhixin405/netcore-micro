using project.Context;
using project.Models;
using project.Repositories;
using project.Repositories;
using RepositoryComponent.BaseRepo;
using RepositoryComponent.DbFactories;

namespace project.Repositories
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
