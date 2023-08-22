using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageMiddleware
{
    /// <summary>
    /// 监听对象
    /// </summary>
    public class ListenerConfig
    {
        public string Queue { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public Func<string, Task<bool>> OnReceivedHandle;
    }
}
