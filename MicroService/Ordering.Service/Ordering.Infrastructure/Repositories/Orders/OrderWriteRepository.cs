using Ordering.Domain.Orders;
using Ordering.Infrastructure.Database;
using RepositoryComponent.BaseRepo;
using RepositoryComponent.DbFactories;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderWriteRepository : WriteRepository<WriteOrderDbContext, Order>, IWriteOrderRepository
    {
        private readonly WriteOrderDbContext _readContext;


        public OrderWriteRepository(DbFactory<WriteOrderDbContext> readContextFactory) : base(readContextFactory)
        {
            _readContext = readContextFactory?.Context;
        }
    }
}
