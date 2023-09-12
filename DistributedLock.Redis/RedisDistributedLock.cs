using System.Net.Http.Headers;
using DistributedLock.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace DistributedLock.Redis
{
    public class RedisDistributedLock : IDistributedLock
    {
        private readonly RedLockFactory _redLockFactory;
        public RedisDistributedLock(IOptions<DistributedLockOptions> options,IServiceProvider serviceProvider)
        {
            var multiplexers = options.Value.RedisEndPoints?.Select(x=>new RedLockMultiplexer(connectionMultiplexer: ConnectionMultiplexer.Connect(x)))
                .ToList()??throw new Exception($"未能获取redis链接{nameof(RedisDistributedLock)}");
            _redLockFactory = RedLockFactory.Create(multiplexers);
            IHostApplicationLifetime hostApplicationLifetime = serviceProvider.GetService<IHostApplicationLifetime>();
            if(hostApplicationLifetime != null )
            {
                hostApplicationLifetime.ApplicationStopping.Register(() =>
                {
                    _redLockFactory.Dispose();
                });
            }
        }
        public async Task<IDisposable> Lock(string key, TimeSpan? timeout = null)
        {
            timeout = timeout ?? TimeSpan.FromSeconds(10);
            var expiry = TimeSpan.FromSeconds(30);
            var retry = TimeSpan.FromSeconds(1);
            var theLock = await _redLockFactory.CreateLockAsync(key, expiry, timeout.Value, retry);
            if (!theLock.IsAcquired)
            {
                throw new Exception($"获取分布式锁失败:{key}");
            }
            return theLock;
        }
    }
}