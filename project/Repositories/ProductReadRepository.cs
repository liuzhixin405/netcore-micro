using project.Context;
using project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RepositoryComponent.BaseRepo;
using RepositoryComponent.DbFactories;

namespace project.Repositories
{
    public class ProductReadRepository : ReadRepository<ReadProductDbContext, Product>, IReadProductRepository
    {
        private readonly ReadProductDbContext _readContext;


        public ProductReadRepository(DbFactory<ReadProductDbContext> readContextFactory) : base(readContextFactory)
        {
            _readContext = readContextFactory?.Context;
        }

        public async ValueTask<Product> GetById(long id)
        {
            var result =await _readContext.Set<Product>().Where(x => x.Id == id).FirstOrDefaultAsync();
            return result;
        }
    }
}
