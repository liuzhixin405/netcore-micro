
using RepositoryComponent.BaseRepo;
using Ordering.Domain.Orders;

namespace Ordering.Infrastructure.Repositories
{
    public interface IReadOrderRepository : IReadRepository<Order>
    {
        ValueTask<Order> GetById(long id);
    }


}
