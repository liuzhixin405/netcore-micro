
using GrpcService.OrderService;
using Ordering.Domain.Enums;
using Ordering.Infrastructure.Repositories;

namespace Ordering.WebApi.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IWriteOrderRepository _writeRepository;
        private readonly IReadOrderRepository _readRepository;
        public OrderService(IWriteOrderRepository writeOrderRepository, IReadOrderRepository readOrderRepository)
        {
            _writeRepository = writeOrderRepository;
            _readRepository = readOrderRepository;
        }
        public async Task<bool> ChangeOrderStaus(long id, int status)
        {
            var order = await _readRepository.GetById(id);
            if (order == null)
            {
                return false;
            }
            order.SetOrderStaus((OrderStatus)status);
            await _writeRepository.UpdateAsync(order);
            var result = await _writeRepository.SaveChangeAsync();
            return result == 1;
        }
    }
}
