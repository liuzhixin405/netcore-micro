using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedLock.Abstractions
{
    public interface IDistributedLock
    {
        Task<IDisposable> Lock(string key,TimeSpan? timeout = null);
    }
}
