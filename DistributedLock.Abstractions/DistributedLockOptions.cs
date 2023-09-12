namespace DistributedLock.Abstractions
{
    public class DistributedLockOptions
    {
        public LockType LockType { get; set; } = LockType.InMemory;
        public string[]? RedisEndPoints { get; set; }
    }
}