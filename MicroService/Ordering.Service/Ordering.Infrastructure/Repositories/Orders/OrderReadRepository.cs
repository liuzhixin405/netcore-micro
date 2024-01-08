using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Ordering.Domain.Orders;
using Ordering.Infrastructure.Database;
using RepositoryComponent.BaseRepo;
using RepositoryComponent.DbFactories;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderReadRepository : ReadRepository<ReadOrderDbContext, Order>, IReadOrderRepository
    {
        private readonly ReadOrderDbContext _readContext;


        public OrderReadRepository(DbFactory<ReadOrderDbContext> readContextFactory) : base(readContextFactory)
        {
            _readContext = readContextFactory?.Context;
        }

        public async ValueTask<Order> GetById(long id)
        {
            var result = await _readContext.Set<Order>().Where(x => x.Id == id).FirstOrDefaultAsync();
            return result;
        }
    }
}
