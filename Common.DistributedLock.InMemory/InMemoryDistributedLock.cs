using AsyncKeyedLock;
using DistributedLock.Abstractions;

namespace DistributedLock.InMemory
{
    public class InMemoryDistributedLock : IDistributedLock
    {
        private readonly StripedAsyncKeyedLocker<string> _asyncKeyedLocker = new();
        public async Task<IDisposable> Lock(string key, TimeSpan? timeout = null)
        {
            timeout = timeout ?? TimeSpan.FromSeconds(10);

            var lockResult = await _asyncKeyedLocker.LockAsync(key, timeout.Value);
            if (!lockResult.EnteredSemaphore)
            {
                throw new TimeoutException($"获取锁超时{timeout.Value.TotalSeconds}秒");
            }

            return lockResult;
        }
    }
}