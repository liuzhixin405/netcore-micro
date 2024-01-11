
using Common.DistributedId;
using Grpc.Net.Client;
using GrpcService.OrderService;
using MagicOnion.Client;
using Microsoft.EntityFrameworkCore;
using Paying.WebApi.Database;

namespace Paying.WebApi.Services
{
    public class PayingService : IPayingService
    {
        private readonly GrpcChannel _channel;
        private PaymentContext _context;
        private IDistributedId _distributedId;
        public PayingService(IConfiguration configuration, PaymentContext context, IDistributedId distributedId)
        {
            _channel = GrpcChannel.ForAddress(configuration["GrpcServices:Order"] ?? throw new ArgumentNullException("order grpcaddress is null"));
            _context = context;
            _distributedId = distributedId;
        }

        public async Task<bool> Add(long orderId)
        {
            await _context.Payments.AddAsync(new Payment { Id = _distributedId.NewLongId(), OrderId = orderId });
            var result = await _context.SaveChangesAsync();
            return result == 1;
        }

        public async Task<bool> ChangeOrderStatus(long orderId, int status)
        {
            //using (_channel)
            {
                try
                {
                    var client = MagicOnionClient.Create<IGrpcOrderService>(_channel);
                    var result = await client.ChangeOrderStatus(new ChangeOrderStatusRequest(orderId, status));
                    return result.success;
                }
                catch (Exception ex)
                {
                    throw new Exception($"获取用户信息出错:{ex.Message}");
                }
            }
        }

        public async Task<int> GetOrderStatus(long orderId)
        {
            //using (_channel)
            {
                try
                {
                    var client = MagicOnionClient.Create<IGrpcOrderService>(_channel);
                    var result = await client.GetOrderStatus(new GetOrderStatusRequest(orderId));
                    return result.status;
                }
                catch (Exception ex)
                {
                    throw new Exception($"获取用户信息出错:{ex.Message}");
                }
            }
        }

        public async Task<bool> IsPay(long orderId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(x => x.OrderId == orderId);
            return payment != null;
        }
    }
}
