
using System.Text;
using System.Threading.Channels;
using Microsoft.AspNetCore.Connections;
using Paying.WebApi.Dtos;
using Paying.WebApi.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Paying.WebApi.BackServices
{
    public class ConsumerService : BackgroundService //给payconfirm 发送一条消息需要订单id
    {
        private readonly IModel channel;
        private readonly IConnection connection;
        private EventingBasicConsumer consumer;
        private readonly IPayingService _payingService;
        public ConsumerService(IServiceProvider sp)
        {
            _payingService = sp.CreateScope().ServiceProvider.GetService<IPayingService>(); ;
            RabbitMqConfig rabbitMqConfig = new(); 
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = rabbitMqConfig.Host;
            factory.Port = rabbitMqConfig.Port;
            factory.UserName = rabbitMqConfig.UserName;
            factory.Password = rabbitMqConfig.Password;
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            var queueName = Const.Normal_Queue;
            channel.ExchangeDeclare(Const.Normal_Exchange, ExchangeType.Fanout, true);
            channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>
                        {
                            { "x-message-ttl" ,Const.DelayTime},
                            {"x-dead-letter-exchange",Const.Delay_Exchange },
                            {"x-dead-letter-routing-key",Const.Delay_RoutingKey }
                        });

            channel.QueueBind(queueName,Const.Normal_Exchange , "");
            //输入1，那如果接收一个消息，但是没有应答，则客户端不会收到下一个消息
            channel.BasicQos(0, 1, false);
            //在队列上定义一个消费者
             consumer = new EventingBasicConsumer(channel);
           

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("normal Rabbitmq消费端开始工作!");
            while (!stoppingToken.IsCancellationRequested)
            {
                channel.BasicConsume(Const.Normal_Queue, false, consumer);
                consumer.Received += async (ch, ea) =>
                {
                    byte[] bytes = ea.Body.ToArray();
                    string str = Encoding.UTF8.GetString(bytes);
                    CreateOrderEvent createOrderEvent = System.Text.Json.JsonSerializer.Deserialize<CreateOrderEvent>(str);
                    var orderStatus = await _payingService.GetOrderStatus(createOrderEvent.EventId);

                    Console.WriteLine($"{DateTime.Now}来自支付获取的消息: {str.ToString()}");
                    //回复确认
                    if (orderStatus == 2) //待付款
                    {
                        Console.WriteLine($"{DateTime.Now}来自支付获取的消息: {str.ToString()},该消息被拒绝");
                        channel.BasicNack(ea.DeliveryTag, false, false);
                    }
                    else  //正常消息处理
                    {
                        Console.WriteLine($"{DateTime.Now}来自支付获取的消息: {str.ToString()}，该消息被接受");
                        if (await _payingService.IsPay(createOrderEvent.EventId) && orderStatus == 2)
                        {
                            var result = await _payingService.ChangeOrderStatus(createOrderEvent.EventId, 3); //付款完成
                        }
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                };

                await Task.Delay(1000*60);
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
