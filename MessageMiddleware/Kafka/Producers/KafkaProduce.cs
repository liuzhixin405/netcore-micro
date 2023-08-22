using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageMiddleware.Kafka.Producers
{
    public class KafkaProduce : IKafkaProduce
    {
        private readonly ProducerOptions _options;

        public KafkaProduce(IOptionsMonitor<ProducerOptions> options)
        {
            _options = options.CurrentValue;
        }

        public async Task PublishAsync<TMessage>(string topic, TMessage message) where TMessage : class
        {
            using var producer = CreateProducerBuilder();
            await producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = _options.Key,
                Value = message.ToJson()
            });
        }

        public async Task PublishAsync<TMessage>(string topic, string key, TMessage message) where TMessage : class
        {
            var producer = CreateProducerBuilder();
            await producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = key,
                Value = message.ToJson()
            });
        }

        public async Task PublishAsync(string topic, string message)
        {
            var producer = CreateProducerBuilder();
            await producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = _options.Key,
                Value = message
            });
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
