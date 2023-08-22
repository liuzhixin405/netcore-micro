using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using MessageMiddleware.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageMiddleware.Kafka.Producers
{
    /// <summary>
    /// 
    /// </summary>
    public class KafkaPublisher : IMQPublisher
    {
        private readonly ProducerOptions _options;
        private readonly ILogger<KafkaPublisher> _logger;

        public KafkaPublisher(ILogger<KafkaPublisher> logger, ProducerOptions options)
        {
            _logger = logger;
            _options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        public bool Publish<T>(T obj, string exchange, string queue = "", string routingKey = "#", JsonSerializerSettings settings = null)
        {
            var producer = CreateProducerBuilder();
            try
            {
                var content = JsonConvert.SerializeObject(obj, settings);
                producer.Produce(exchange, new Message<string, string>
                {
                    Key = exchange,
                    Value = content
                });
                _logger.LogInformation($"exchange:{exchange},routingKey:{routingKey},content:{content}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "kafka推送失败");
                return false;
            }
            finally
            {
                producer?.Dispose();
            }
            return true;
        }

        private IProducer<string, string> CreateProducerBuilder()
        {
            var config = new ProducerConfig();
            config.BootstrapServers = _options.BootstrapServers;

            if (!string.IsNullOrEmpty(_options.SaslUsername))
            {
                config.SaslUsername = _options.SaslUsername;
                config.SaslPassword = _options.SaslPassword;
                config.SaslMechanism = SaslMechanism.Plain;
                config.SecurityProtocol = SecurityProtocol.SaslPlaintext;
            }
            return new ProducerBuilder<string, string>(config).Build();
        }
    }
}
