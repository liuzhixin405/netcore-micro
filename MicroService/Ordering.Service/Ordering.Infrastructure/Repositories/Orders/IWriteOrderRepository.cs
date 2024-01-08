using RepositoryComponent.BaseRepo;
using Ordering.Domain.Orders;

namespace Ordering.Infrastructure.Repositories
{
    public interface IWriteOrderRepository : IWriteRepository<Order>
    {

    }
}
