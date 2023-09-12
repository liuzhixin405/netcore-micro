using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedLock.Abstractions
{
    /// <summary>
    /// 分布式锁类型
    /// </summary>
    public enum LockType
    {
        /// <summary>
        /// 内存实现(进程内有效)
        /// </summary>
        InMemory = 1,
        /// <summary>
        /// Redis实现（分布式）
        /// </summary>
        Redis =2
    }
}
