using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedLock.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.DistributedId
{
    internal class DistributedId : IDistributedId
    {
        private IdWorker _idWorker;
        private readonly object _locker = new object();
        private readonly DistributedIdOptions _distributedIdOption;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;
        public DistributedId(IOptions<DistributedIdOptions> distributedIdOption,IServiceProvider serviceProvider)
        {
            _distributedIdOption = distributedIdOption.Value;
            _serviceProvider = serviceProvider;
        }
        public Guid NewGuid()
        {
            return GuidHelper.NewGuid(_distributedIdOption.GuidType);
        }

        public Guid NewGuid(SequentialGuidType sequentialGuidType)
        {
            return GuidHelper.NewGuid(sequentialGuidType);
        }

        public long NewLongId()
        {
            if(_idWorker == null)
            {
                lock(_locker)
                {
                    if (_idWorker == null)
                    {
                        var workerId = AsyncHelper.RunSync(() => GetWorkerId());
                        _idWorker = new IdWorker(workerId);
                    }
                }
            }
            return _idWorker.NextId();
        }

        private async Task<int> GetWorkerId()
        {
            if (_distributedIdOption.Distributed)
            {
                return _distributedIdOption.WorkerId==0 ? new Random().Next(1,1024):_distributedIdOption.WorkerId;
            }

            IDistributedCache distributedCache = _serviceProvider.GetService<IDistributedCache>()??throw new ArgumentNullException($"未能获取{nameof(IDistributedCache)}实现");

            IDistributedLock distirbutedLock = _serviceProvider.GetService<IDistributedLock>() ?? throw new ArgumentNullException($"未能获取{nameof(IDistributedLock)}实现");
            for(int i = 1; i < 1024; i++)
            {
                var lockKey = $"{GetType().FullName}:WorkerIdLock:{i}";
                using var _ = distirbutedLock.Lock(lockKey);

                var key = $"{GetType().FullName}:WorkerId:{i}";
                var value = await distributedCache.GetStringAsync(key);
                if (string.IsNullOrEmpty(value))
                {
                    await distributedCache.SetStringAsync(key, DateTimeOffset.Now.ToString(), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });

                    _timer = new Timer(State =>
                    {
                        distributedCache.SetString(key, DateTimeOffset.Now.ToString(), new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                        });
                    },null,0,1000);
                    return i;
                }
            }
            throw new Exception("WorkerId已用完");
        }
    }
}
