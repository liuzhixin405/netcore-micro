using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MessageMiddleware.Kafka.Producers;
using MessageMiddleware.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageMiddleware.Factory
{
    /// <summary>
    /// mq工厂实现
    /// </summary>
    public class MQFactory : IMQFactory
    {
        IServiceProvider _serviceProvider;
        Func<string, IMQPublisher> _publisher;

        public MQFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _publisher = serviceProvider.GetService<Func<string, IMQPublisher>>();
        }

        /// <summary>
        /// 根据需求创建对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IMQPublisher Create(MQType type)
        {
            IMQPublisher mq = null;
            switch (type)
            {
                case MQType.Rabbit:
                    mq = _publisher(MQType.Rabbit.ToString());
                    break;
                case MQType.Kafka:
                    mq = _publisher(MQType.Kafka.ToString());
                    break;
                default:
                    throw new ArgumentException($"Not Support Servivce，{nameof(MQFactory)}--{nameof(Create)}");
            }
            return mq;
        }
    }
}
