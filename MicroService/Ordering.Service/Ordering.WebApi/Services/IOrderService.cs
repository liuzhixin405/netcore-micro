using Ordering.Domain.Dtos;
using Ordering.Domain.Orders;

namespace Ordering.WebApi.Services
{
    public interface IOrderService
    {
        Task<bool> Create(CreateOrderDto orderDto);
        Task<List<OrderDetailDto>> GetOrderDetails();
    }
}
