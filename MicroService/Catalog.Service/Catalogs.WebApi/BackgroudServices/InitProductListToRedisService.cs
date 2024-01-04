using Catalogs.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
        public InitProductListToRedisService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configuration["DistributedRedis:ConnectionString"] ?? throw new Exception("$未能获取distributedredis连接字符串"));
            _redisDb = redis.GetDatabase();
            

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            CatalogContext _context = scope.ServiceProvider.GetRequiredService<CatalogContext>();
            ILogger logger = scope.ServiceProvider.GetRequiredService<ILogger<InitProductListToRedisService>>();
            //while(!stoppingToken.IsCancellationRequested)
            {
               var products =await _context.Catalogs.ToListAsync();

                foreach (var product in products)
                {
                    string hashKey = "products";
                    string productField = product.Id.ToString();
                    string productValue = System.Text.Json.JsonSerializer.Serialize(product);

                    _redisDb.HashSet(hashKey, new HashEntry[] { new HashEntry(productField, productValue) });

                }
                logger.LogInformation($"ProductList is over stored in Redis Hash.");
            }
        }
    }
}
