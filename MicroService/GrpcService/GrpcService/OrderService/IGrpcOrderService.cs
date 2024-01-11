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

        UnaryResult<GetOrderStatusResponse> GetOrderStatus(GetOrderStatusRequest request);
        //待付款订单
    }

    [MessagePackObject(true)]
    public record ChangeOrderStatusResponse(bool success,string message);

    [MessagePackObject(true)]
    public record ChangeOrderStatusRequest(long orderId,int status);

    [MessagePackObject(true)]
    public record GetOrderStatusResponse(int status);

    [MessagePackObject(true)]
    public record GetOrderStatusRequest(long orderId);

}
