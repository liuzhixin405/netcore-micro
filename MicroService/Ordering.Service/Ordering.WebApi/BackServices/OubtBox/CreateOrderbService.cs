using System.Text;
using System.Transactions;
using MessageMiddleware;
using Ordering.Dimain.OutBoxMessages;
using Ordering.Domain.Orders;
using Ordering.Infrastructure.Repositories;
using Polly;
using RabbitMQ.Client;
using StackExchange.Redis;
namespace Ordering.WebApi.OutBoxMessageServices
{
    public class CreateOrderbService : BackgroundService
    {
        private readonly IMQPublisher _publisher;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IModel channel;
        private readonly IConnection connection;
        public CreateOrderbService(IMQPublisher publisher, IServiceScopeFactory serviceScopeFactory)
        {
            _publisher = publisher;
            _serviceScopeFactory = serviceScopeFactory;

            RabbitMqConfig rabbitMqConfig = new();
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = rabbitMqConfig.Host;
            factory.Port = rabbitMqConfig.Port;
            factory.UserName = rabbitMqConfig.UserName;
            factory.Password = rabbitMqConfig.Password;
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            
            channel.ExchangeDeclare(Const.Delay_Exchange, ExchangeType.Direct, true);
            channel.QueueDeclare(Const.Delay_Queue, true, false, false, new Dictionary<string, object>
                        {
                            { "x-message-ttl" ,Const.DelayTime},
                            {"x-dead-letter-exchange",Const.Normal_Exchange },
                            {"x-dead-letter-routing-key",Const.Delay_RoutingKey }
                        });

            channel.QueueBind(Const.Delay_Queue, Const.Delay_Exchange, "");    //用了延时队列但没有封装到组件去,偷下懒

            channel.QueueDeclare(Const.Normal_Queue, true, false, false, null);
            channel.ExchangeDeclare(Const.Normal_Exchange, ExchangeType.Fanout, true);
            channel.QueueBind(Const.Normal_Queue, Const.Normal_Exchange, Const.Delay_RoutingKey);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var repository = scope.ServiceProvider.GetService<IReadOutBoxMessageRepository>();
                    var writeRepo = scope.ServiceProvider.GetService<IWriteOutBoxMessageRepository>();

                    var orderRepository = scope.ServiceProvider.GetService<IReadOrderRepository>();
                    var orderWriteRepo = scope.ServiceProvider.GetService<IWriteOrderRepository>();
                    var logger = scope.ServiceProvider.GetService<ILogger<CreateOrderbService>>();
                    var messages = await repository.GetTake(10);
                    List <Domain.Orders.Order> orders = new List<Domain.Orders.Order>(); 
                    foreach (var message in messages)
                    {
                        if (string.IsNullOrEmpty(message.Content))
                            continue;
                        var retries = 3;
                        var retry = Policy.Handle<Exception>()
                            .WaitAndRetry(
                            retries,
                            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                            (exception, timeSpan, retry, ctx) =>
                            {
                                logger.LogError($"发布时间失败:{message}");
                            });
                        retry.Execute(() => _publisher.Publish(new { Content = message.Content, Id = message.Id,Type= message.Type}, exchange: "RabbitMQ.EventBus.CreateOrder",queue: "RabbitMQ.EventBus.CreateOrder"));
                        retry.Execute(() =>
                        {
                            byte[] buffer = Encoding.UTF8.GetBytes(message.Content);
                            IBasicProperties basicProperties = channel.CreateBasicProperties();
                            basicProperties.DeliveryMode = 2; //持久化  1=非持久化
                            channel.BasicPublish(Const.Delay_Exchange, "", basicProperties, buffer);
                            // 在服务结束时关闭连接和通道
                            //channel.Close();
                            //connection.Close();
                        });
                        message.ProceddedOnUtc = DateTime.UtcNow;
                        var order = await orderRepository.GetById(message.Id);
                        order.SetOrderStaus(Domain.Enums.OrderStatus.Pending);
                        orders.Add(order);
                    }
                        //TODO:更新待确认为待付款
                        await writeRepo.UpdateRangeAsync(messages, stoppingToken);
                        await orderWriteRepo.UpdateRangeAsync(orders, stoppingToken);
                        var res = await orderWriteRepo.SaveChangeAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    await Task.Delay(1000 * 60 * 1);
                }
            }
        }
    }
}