using Ordering.Domain.Dtos;

namespace Ordering.WebApi.Services
{
    public interface IOrderService
    {
        Task<bool> Create(CreateOrderDto orderDto);
    }
}
