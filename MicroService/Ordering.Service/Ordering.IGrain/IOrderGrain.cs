using Ordering.Domain.Dtos;
using Orleans;

namespace Ordering.IGrain
{
    public interface IOrderGrain: IGrainWithIntegerKey
    {
        Task<bool> Create(CreateOrderDto orderDto);
        Task<List<OrderDetailDto>> GetOrderDetails();
    }
}
