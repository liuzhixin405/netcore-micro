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
            Exchange = "RabbitMQ.EventBus.CreateOrder";
            Queue = "";
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
            Console.WriteLine($"等待处理的消息{get.Id}_{get.Type}");
            CreateOrderEvent createOrderEvent = System.Text.Json.JsonSerializer.Deserialize<CreateOrderEvent>(get.Content);
           
            var catalog = await _context.Catalogs.FirstOrDefaultAsync(c=>c.Id== createOrderEvent.ProductId);
            var updateResult = await catalog.RemoveStock(createOrderEvent.Quantity);
            if (!updateResult.Item1)
            {
                throw new Exception($"更新库存失败:{updateResult.Item2}");
            }
            _context.Catalogs.Update(catalog);
            Console.WriteLine($"处理的消息完毕");

            _context.Set<OutboxMessageConsumer>().Add(new OutboxMessageConsumer
            {
                Id = get.Id,
                Name = get.Type
            });

            var result = await _context.SaveChangesAsync();
            return result != -1;
        }
    }
}
