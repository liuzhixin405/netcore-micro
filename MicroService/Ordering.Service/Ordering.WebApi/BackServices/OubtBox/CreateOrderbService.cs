using MessageMiddleware;
using Ordering.Dimain.OutBoxMessages;
using Ordering.Infrastructure.Repositories;
using Polly;
namespace Ordering.WebApi.OutBoxMessageServices
{
    public class CreateOrderbService : BackgroundService
    {
        private readonly IMQPublisher _publisher;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CreateOrderbService(IMQPublisher publisher, IServiceScopeFactory serviceScopeFactory)
        {
            _publisher = publisher;
            _serviceScopeFactory = serviceScopeFactory;
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
                    var messages = await repository.GetTake(10);
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
                                Console.WriteLine($"发布时间失败:{message}");
                            });
                        retry.Execute(() => _publisher.Publish(new { Content = message.Content, Id = message.Id,Type= message.Type}, exchange: "RabbitMQ.EventBus.CreateOrder",queue: "RabbitMQ.EventBus.CreateOrder"));
                        message.ProceddedOnUtc = DateTime.UtcNow;
                    }
                    //TODO:更新待确认为待付款
                    await writeRepo.UpdateRangeAsync(messages, stoppingToken);
                    var res = await writeRepo.SaveChangeAsync(stoppingToken);
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