using Redis.Extensions;
using Redis.Extensions.Configuration;
using StackExchange.Redis;

namespace project.Utility.Helper
{
    public interface IProductRedis : IRedisCache { }
    public class ProductRedis:RedisCache,IProductRedis
    {
        public ProductRedis(ILogger<ProductRedis> logger,
           IConnectionMultiplexer connection,
           RedisConfiguration config,
           ISerializer serializer) : base(logger, connection, config, serializer)
        {

        }
    }
}
