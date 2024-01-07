using DistributedId;
using Ordering.Domain.Dtos;
using Ordering.Domain.Orders;
using Ordering.Infrastructure.Repositories;

namespace Ordering.WebApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IWriteOrderRepository _writeRepo;
        private readonly IReadOrderRepository _readRepo;
        private readonly IDistributedId _distributedId;
        public OrderService(IWriteOrderRepository writeOrderRepository, IReadOrderRepository readOrderRepository, IDistributedId distributedId)
        {
            _writeRepo = writeOrderRepository;
            _readRepo = readOrderRepository;
            _distributedId = distributedId;
        }
        public async Task<bool> Create(CreateOrderDto orderDto)
        {
            //新增订单
           decimal totalAmount = orderDto.price * orderDto.quantity ;
            long userId = 1L;
            await _writeRepo.AddAsync(Order.CreateNew(_distributedId.NewLongId(),userId, orderDto.pid, orderDto.quantity, totalAmount));
            var result = await _writeRepo.SaveChangeAsync();
            //扣减库存

            //通知付款
            return result != -1;
        }
    }
}
