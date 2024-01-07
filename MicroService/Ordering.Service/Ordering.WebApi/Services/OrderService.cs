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
        private readonly IHttpContextAccessor _contextAccessor;
        public OrderService(IWriteOrderRepository writeOrderRepository, IReadOrderRepository readOrderRepository, IDistributedId distributedId, IHttpContextAccessor contextAccessor)
        {
            _writeRepo = writeOrderRepository;
            _readRepo = readOrderRepository;
            _distributedId = distributedId;
            _contextAccessor = contextAccessor;
        }
        public async Task<bool> Create(CreateOrderDto orderDto)
        {
            
            long userId = long.Parse(_contextAccessor.HttpContext.Items["UserId"]?.ToString());
            if(userId == 0)
            {
                return false;
            }
            //新增订单
            decimal totalAmount = orderDto.price * orderDto.quantity ;
            
            await _writeRepo.AddAsync(Order.CreateNew(_distributedId.NewLongId(),userId, orderDto.pid, orderDto.quantity, totalAmount));
            var result = await _writeRepo.SaveChangeAsync();
            //扣减库存

            //通知付款
            return result != -1;
        }
    }
}
