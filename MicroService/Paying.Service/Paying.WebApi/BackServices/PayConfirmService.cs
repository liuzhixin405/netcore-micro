
using System.Threading.Channels;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace Paying.WebApi.BackServices
{
    public class PayConfirmService : BackgroundService
    {
        private readonly Channel<string> _payChannel;
        private readonly IModel channel;
        private readonly IConnection connection;
        public PayConfirmService(Channel<string> payChannel)
        {
            _payChannel = payChannel;
            RabbitMqConfig rabbitMqConfig = new();
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = rabbitMqConfig.Host;
            factory.Port = rabbitMqConfig.Port;
            factory.UserName = rabbitMqConfig.UserName;
            factory.Password = rabbitMqConfig.Password;
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            var queueName = Const.Normal_Queue; ;
            channel.ExchangeDeclare(Const.Normal_Exchange, ExchangeType.Fanout, true);
            channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>
                        {
                            { "x-message-ttl" ,Const.DelayTime},
                            {"x-dead-letter-exchange",Const.Delay_Exchange },
                            {"x-dead-letter-routing-key",Const.Delay_RoutingKey }
                        });

            channel.QueueBind(queueName, Const.Normal_Exchange, "");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            while (!_payChannel.Reader.Completion.IsCompleted)
            {
                var msg = await _payChannel.Reader.ReadAsync();
                if (msg == "pay")
                {
                    string sendMessage = string.Format("{0}", "pay");
                    byte[] buffer = Encoding.UTF8.GetBytes(sendMessage);
                    IBasicProperties basicProperties = channel.CreateBasicProperties();
                    basicProperties.DeliveryMode = 2; //持久化  1=非持久化
                    channel.BasicPublish(Const.Normal_Exchange, Const.Normal_RoutingKey, basicProperties, buffer);
                    Console.WriteLine($"{DateTime.Now}消息发送成功：{sendMessage}");
                }
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
