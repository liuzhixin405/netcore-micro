using System.Reflection;
using System.Threading.Channels;
using Catalogs.Infrastructure.Database;
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
        private readonly IDatabase _redisDb;
        private readonly Channel<string> _channel;
        private readonly ILogger _logger;
        public InitProductListToRedisService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, Channel<string> channel, ILogger<InitProductListToRedisService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configuration["DistributedRedis:ConnectionString"] ?? throw new Exception("$未能获取distributedredis连接字符串"));
            _redisDb = redis.GetDatabase();
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
               
                   await _redisDb.KeyDeleteAsync(hashKey);
                
                    foreach (var product in products)
                    {
                        
                        string productField = product.Id.ToString();
                        string productValue = System.Text.Json.JsonSerializer.Serialize(product);

                        _redisDb.HashSet(hashKey, new HashEntry[] { new HashEntry(productField, productValue) });
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
