using MessageMiddleware.Kafka.Producers;
using MessageMiddleware.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageMiddleware.Kafka.Consumer;

namespace MessageMiddleware.Factory
{
    /// <summary>
    /// 
    /// </summary>
    public class MQConfig
    {
        /// <summary>
        /// 使用哪个mq，对应MQType枚举
        /// 1：kafka
        /// 2：rabbit
        /// </summary>
        public int Use { get; set; }

        public bool PublishLog { get; set; } = true;
        /// <summary>
        /// 
        /// </summary>
        public bool ConsumerLog { get; set; } = true;

        /// <summary>
        /// 生产配置
        /// </summary>
        public ProducerOptions Kafka { get; set; }

        /// <summary>
        /// rabbit配置
        /// </summary>
        public RabbitMQSetting Rabbit { get; set; }
    }
}
