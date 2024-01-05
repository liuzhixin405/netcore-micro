using System.Reflection;
using System.Threading.Channels;
using Catalogs.Infrastructure.Database;
using Common.Redis.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Catalogs.WebApi.BackgroudServices
{
    /// <summary>
    /// 记得任何删除了或者购买了产品后需要删除改产品的键
    /// </summary>
    public class InitProductListToRedisService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRedisCache _redisDb;
        private readonly Channel<string> _channel;
        private readonly ILogger _logger;
        public InitProductListToRedisService(IServiceScopeFactory serviceScopeFactory, IRedisCache redisCache, Channel<string> channel, ILogger<InitProductListToRedisService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _redisDb = redisCache;
            _channel = channel;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Init();

            while (!_channel.Reader.Completion.IsCompleted)
            {
                var msg = await _channel.Reader.ReadAsync();
                if(msg == "delete_catalog_fromredis")
                {
                    await Init();
                }
            }
        }

        private async Task Init()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            try
            {
                CatalogContext _context = scope.ServiceProvider.GetRequiredService<CatalogContext>();
                string hashKey = "products";
                var products = await _context.Catalogs.ToListAsync();
               
                   await _redisDb.HDelAsync(hashKey);
                
                    foreach (var product in products)
                    {
                        
                        string productField = product.Id.ToString();
                        string productValue = System.Text.Json.JsonSerializer.Serialize(product);

                       await _redisDb.HMSetAsync(hashKey, new HashEntry[] { new HashEntry(productField, productValue) });
                    }

                    _logger.LogInformation($"ProductList is over stored in Redis Hash.");           
            }
            catch(Exception ex)
            {
                _logger.LogError($"ProductLis stored in Redis Hash error.");
            }
        }
    }
}
