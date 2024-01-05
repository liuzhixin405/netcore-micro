using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using MessageMiddleware.Factory;

namespace MessageMiddleware.RabbitMQ
{
    public class MQPublisher : IMQPublisher
    {
        private readonly DefaultObjectPool<IModel> _channelPool;
        private readonly ILogger<MQPublisher> _logger;
        private readonly MQConfig _config;

        public MQPublisher(IRabbitMQConnection mqConnection, ILogger<MQPublisher> logger
            , MQConfig config
            )
        {
            _logger = logger;
            _config = config;

            _channelPool =
                new DefaultObjectPool<IModel>(new RabbitMQChannelPooledObjectPolicy(mqConnection, _logger), Environment.ProcessorCount * 2);
        }

        /// <summary>
        /// 发送队列消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routingKey"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public bool Publish<T>(T obj, string exchange, string queue = "", string routingKey = "#", JsonSerializerSettings settings = null)
        {
            var channel = _channelPool.Get();
            try
            {
                var content = JsonConvert.SerializeObject(obj, settings);
                var body = Encoding.UTF8.GetBytes(content);
                var props = channel.CreateBasicProperties();
                props.DeliveryMode = 2;//1：非持久化 2：可持久化

                channel.ExchangeDeclare(exchange, "topic", true, false, null);

                if (!string.IsNullOrWhiteSpace(queue))
                {
                    channel.QueueDeclare(queue, true, false, false, null);
                    channel.QueueBind(queue, exchange, routingKey);
                }

                channel.BasicPublish(exchange, routingKey, false, props, body);

                if (_config.PublishLog)
                {
                    _logger.LogInformation($"exchange:{exchange},routingKey:{routingKey},content:{content}");
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "rabbitMQ推送失败");
                return false;
            }
            finally
            {
                _channelPool.Return(channel);
            }

            return true;
        }
    }
}
