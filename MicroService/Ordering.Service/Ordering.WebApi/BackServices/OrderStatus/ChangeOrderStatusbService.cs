
using Ordering.WebApi.Services.Orders;

namespace Ordering.WebApi.BackServices.OrderStatus
{
    public class ChangeOrderStatusbService : BackgroundService
    {
        private readonly IOrderService _orderService;
        public ChangeOrderStatusbService(IOrderService orderService)
        {
            _orderService = orderService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {


                await Task.Delay(1000*60); //一分钟一次
            }
        }
    }
}
