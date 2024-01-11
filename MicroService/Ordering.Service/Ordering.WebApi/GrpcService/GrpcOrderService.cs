using Grpc.Core;
using GrpcService.CustomerService;
using GrpcService.OrderService;
using MagicOnion;
using MagicOnion.Server;
using NJsonSchema.Validation;
using Ordering.Domain.Enums;
using Ordering.Infrastructure.Repositories;

namespace Ordering.WebApi.GrpcService
{
    public class GrpcOrderService : ServiceBase<IGrpcOrderService>, IGrpcOrderService
    {
        private readonly IWriteOrderRepository _writeRepository;
        private readonly IReadOrderRepository _readRepository;
        public GrpcOrderService(IWriteOrderRepository writeOrderRepository,IReadOrderRepository readOrderRepository)
        {
            _writeRepository = writeOrderRepository;
            _readRepository = readOrderRepository;
        }
        public async UnaryResult<ChangeOrderStatusResponse> ChangeOrderStatus(ChangeOrderStatusRequest request)
        {
          var order = await _readRepository.GetById(request.orderId);
            if(order == null) {
            return new ChangeOrderStatusResponse(false,"该订单不存在");
            }
            order.SetOrderStaus((OrderStatus)request.status);
            await _writeRepository.UpdateAsync(order);
            var result = await _writeRepository.SaveChangeAsync();
            return new ChangeOrderStatusResponse(result==1, "");
        }
        public async UnaryResult<GetOrderStatusResponse> GetOrderStatus(GetOrderStatusRequest request)
        {
            var order = await _readRepository.GetById(request.orderId);
            return new GetOrderStatusResponse((int)order.OrderStatus);
        }
    }
}
