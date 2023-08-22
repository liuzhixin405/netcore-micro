using System.Text;
using System.Text.Json.Serialization;
using MessagePack;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;

namespace project.Utility.Helper
{
    public class CacheHelper
    {
        private static IRedisClientFactory _factory_with_msgpack;
        private static IRedisDatabase _redis_with_msgpack => _factory_with_msgpack.GetDefaultRedisDatabase();

        private static IRedisClientFactory _factory;
        private static IRedisDatabase _redis => _factory.GetDefaultRedisDatabase();
        public static void Init(IConfiguration configuration)
        {
            var config = configuration.GetSection("Redis").Get<RedisConfiguration>();
            _factory = new RedisClientFactory(new[] { config }, null, new RedisSerializer());
            _factory_with_msgpack = new RedisClientFactory(new[] { config }, null, new RedisMessagepackSerializer());
        }
        static CacheHelper() { }

        public static T Get<T>(string key)
        {
            return _redis.GetAsync<T>(key).GetAwaiter().GetResult();
        }
        public static async Task<T> GetAsync<T>(string key)
        {
            return await _redis.GetAsync<T>(key);
        }
        public static async Task<T> GetAsync_With_Msgpack<T>(string key)
        {
            return await _redis_with_msgpack.GetAsync<T>(key);
        }

        public static string Get(string key)
        {
            return _redis.GetAsync<string>(key).GetAwaiter().GetResult();
        }

        public static bool Set(string key, object value, TimeSpan expiresIn)
        {
            return _redis.AddAsync(key, value, expiresIn).GetAwaiter().GetResult();
        }
        public static async Task<bool> SetAsync(string key, object value, TimeSpan expiresIn)
        {
            return await _redis.AddAsync(key, value, expiresIn);
        }

        public static async Task<bool> SetAsync_With_Msgpack(string key, object value, TimeSpan expiresIn)
        {
            return await _redis_with_msgpack.AddAsync(key, value, expiresIn);
        }

        /// <summary>
        /// 以秒为单位，返回给定 key 的剩余生存时间
        /// </summary>

        public static long GetExpirin(string key)
        {
            var timespan = _redis.Database.KeyTimeToLive(key);
            if (timespan == null) { return 0; }
            return (long)timespan.Value.TotalSeconds;
        }
        public static bool KeyExpire(string key, TimeSpan expiresIn)
        {
            return _redis.Database.KeyExpire(key, expiresIn);
        }
        public static async Task<bool> RemoveKeyAsync(string key)
        {
            return await _redis.Database.KeyDeleteAsync(key);
        }
        public static long RemoveKey(string key)
        {
            var result = _redis.Database.KeyDelete(key);
            return result ? 1 : 0;
        }
    }


    public class RedisSerializer : ISerializer
    {
        public T? Deserialize<T>(byte[] serializedObject)
        {
            var data = Encoding.UTF8.GetString(serializedObject);
            return System.Text.Json.JsonSerializer.Deserialize<T>(data);
        }

        public byte[] Serialize<T>(T? item)
        {
            var data = System.Text.Json.JsonSerializer.Serialize(item);
            return Encoding.UTF8.GetBytes(data);
        }
    }

    public class RedisMessagepackSerializer : ISerializer
    {
        private MessagePackSerializerOptions _options;
        public RedisMessagepackSerializer()
        {
            _options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
        }
        public T? Deserialize<T>(byte[] serializedObject)
        {
            return MessagePackSerializer.Deserialize<T>(serializedObject, _options);
        }

        public byte[] Serialize<T>(T? item)
        {
            return MessagePackSerializer.Serialize(item, _options);
        }
    }
}
