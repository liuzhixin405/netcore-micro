using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Paying.WebApi;
using Paying.WebApi.Dtos;
using Paying.WebApi.Services;

namespace Paying.WebApi.BackServices
{
    public class PayTimeoutService : BackgroundService //支付超时取消订单
    {
        private readonly IModel channel;
        private readonly IConnection connection;
        private EventingBasicConsumer consumer;
        private readonly IPayingService _payingService;
        public PayTimeoutService(IServiceProvider sp)
        {
            _payingService = sp.CreateScope().ServiceProvider.GetService<IPayingService>();
            RabbitMqConfig rabbitMqConfig = new();
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = rabbitMqConfig.Host;
            factory.Port = rabbitMqConfig.Port;
            factory.UserName = rabbitMqConfig.UserName;
            factory.Password = rabbitMqConfig.Password;
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            //在队列上定义一个消费者
            consumer = new EventingBasicConsumer(channel);

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("取消超时未支付服务开始工作!");
            while (!stoppingToken.IsCancellationRequested)
            {

                channel.BasicConsume(Const.Normal_Queue, false, consumer);
                consumer.Received += (ch, ea) =>
                {
                    byte[] bytes = ea.Body.ToArray();
                    string message = Encoding.UTF8.GetString(bytes);
                    CreateOrderEvent createOrderEvent = System.Text.Json.JsonSerializer.Deserialize<CreateOrderEvent>(message);
                    var orderStatus = _payingService.GetOrderStatus(createOrderEvent.EventId).GetAwaiter().GetResult();
                    if (orderStatus == 2)
                    {
                        var result = _payingService.ChangeOrderStatus(createOrderEvent.EventId, 7).GetAwaiter().GetResult();
                        if (result)
                        {
                            //恢复库存
                        } 
                        Console.WriteLine($"{DateTime.Now}超时未处理的消息id: {createOrderEvent.EventId},处理结果为:{result}");
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    //回复确认
                };
                await Task.Delay(1000 * 60);
            }
        }

        public override void Dispose()
        {
            // 在服务结束时关闭连接和通道
            channel.Close();
            connection.Close();
            base.Dispose();
        }
    }
}
