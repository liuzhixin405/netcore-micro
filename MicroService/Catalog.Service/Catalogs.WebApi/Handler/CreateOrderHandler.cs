using Catalogs.Domain.Catalogs;
using Catalogs.Domain.Events;
using Catalogs.Domain.OutBoxMessage;
using Catalogs.Infrastructure.Database;
using MessageMiddleware;
using Microsoft.EntityFrameworkCore;

namespace Catalogs.WebApi
{
    public class CreateOrderHandler : MQListener
    {
        private readonly IServiceProvider _serviceProvider;
        public CreateOrderHandler(IServiceProvider services) : base(services)
        {
            _serviceProvider = services;
        }

        public override bool Init()
        {
            Exchange = "RabbitMQ.EventBus.CreateOrder";//提前rabbitmq增加一个exchange或者发布消息先
            Queue = "RabbitMQ.EventBus.CreateOrder";
            return true;
        }

        public async override Task<bool> Process(string message)
        {
            // Content = message.Content, Id = message.Id,Type= message.Type
            using var scope = _serviceProvider.CreateScope();
            CatalogContext _context = scope.ServiceProvider.GetService<CatalogContext>();
            ReceiveCreateOrderEvent get = System.Text.Json.JsonSerializer.Deserialize<ReceiveCreateOrderEvent>(message); 
            
            if (await _context.Set<OutboxMessageConsumer>().AnyAsync(o => o.Id ==get.Id && o.Name == get.Type))
            {
                return default;
            }
            var outBoxMessageConsumer = new OutboxMessageConsumer();
            Console.WriteLine($"等待处理的消息{get.Id}_{get.Type}");
            CreateOrderEvent createOrderEvent = System.Text.Json.JsonSerializer.Deserialize<CreateOrderEvent>(get.Content);
           
            var catalog = await _context.Catalogs.FirstOrDefaultAsync(c=>c.Id== createOrderEvent.ProductId);
            if(catalog == null)
            {
                outBoxMessageConsumer.Error = $"{createOrderEvent.ProductId} 不存在catalog中";
                var goout = await _context.SaveChangesAsync(false);
                return goout != -1;
            }

            var updateResult = await catalog.RemoveStock(createOrderEvent.Quantity);
            if (!updateResult.Item1)
            {
                outBoxMessageConsumer.Error = $"更新库存失败:{updateResult.Item2}";
            }
            else
            {
                _context.Catalogs.Update(catalog);
                outBoxMessageConsumer.Id = get.Id;
                outBoxMessageConsumer.Name =$"{get.Type}: 更新{catalog.Id}_库存-{createOrderEvent.Quantity},时间:{DateTime.UtcNow}";
            }
            _context.OutBoxMessageConsumers.Add(outBoxMessageConsumer);
           var result = await _context.SaveChangesAsync(false);
            return result != -1;
        }
    }
}
