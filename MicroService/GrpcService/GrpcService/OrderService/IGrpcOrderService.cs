using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcService.CustomerService;
using MagicOnion;
using MessagePack;

namespace GrpcService.OrderService
{
    public interface IGrpcOrderService:IService<IGrpcOrderService>
    {
        UnaryResult<ChangeOrderStatusResponse> ChangeOrderStatus(ChangeOrderStatusRequest request);

        UnaryResult<CheckOrderOutBoxMessageResponse> CheckOrderOutBoxMessage(CheckOrderOutBoxMessageRequest request);
    }

    [MessagePackObject(true)]
    public record ChangeOrderStatusResponse(bool success,string message);

    [MessagePackObject(true)]
    public record ChangeOrderStatusRequest(long orderId,int status);

    [MessagePackObject(true)]
    public record CheckOrderOutBoxMessageResponse(bool success);

    [MessagePackObject(true)]
    public record CheckOrderOutBoxMessageRequest(long orderId);

}
