using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageMiddleware.Factory
{
    /// <summary>
    /// MQ工厂
    /// </summary>
    public interface IMQFactory
    {
        /// <summary>
        /// 创建mq对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IMQPublisher Create(MQType type);
    }
}
