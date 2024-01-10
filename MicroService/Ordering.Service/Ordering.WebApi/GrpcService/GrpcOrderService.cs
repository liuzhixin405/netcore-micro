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
        private readonly IReadOutBoxMessageRepository _readOutBoxMessageRepository;
        public GrpcOrderService(IWriteOrderRepository writeOrderRepository,IReadOrderRepository readOrderRepository, IReadOutBoxMessageRepository readOutBoxMessageRepository)
        {
            _writeRepository = writeOrderRepository;
            _readRepository = readOrderRepository;
            _readOutBoxMessageRepository = readOutBoxMessageRepository;

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

        public async UnaryResult<CheckOrderOutBoxMessageResponse> CheckOrderOutBoxMessage(CheckOrderOutBoxMessageRequest request)
        {
            var message =await _readOutBoxMessageRepository.GetById(request.orderId);
            return new CheckOrderOutBoxMessageResponse(message==null?false:message.ProceddedOnUtc!=null);
        }//orderstatus新增待确认到待付款状态，可以通过状态判断,也可以继续保持
    }
}
