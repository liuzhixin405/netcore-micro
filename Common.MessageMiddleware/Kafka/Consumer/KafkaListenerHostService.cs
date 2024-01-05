using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MessageMiddleware.Kafka.Producers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageMiddleware.Kafka.Consumer
{
    /// <summary>
    /// Kafka 服务
    /// </summary>
    public class KafkaListenerHostService
    {
        public string Queue { get; set; }
        public string GroupId = "kafka";

        public Func<string, Task<bool>> OnReceivedHandle;

        private readonly ProducerOptions _options;
        private readonly ILogger<KafkaListenerHostService> _logger;

        public KafkaListenerHostService(IServiceProvider services)
        {
            _logger = services.GetRequiredService<ILogger<KafkaListenerHostService>>();
            _options = services.GetRequiredService<ProducerOptions>();
        }

        public void Consumer(bool queueLog=true)
        {
            using var consumer = CreateConsumerBuilder();
            try
            {
                consumer.Subscribe(Queue);

                while (true)
                {
                    var consume = consumer.Consume();
                    var content = consume.Message.Value;
                    if (OnReceivedHandle != null)
                    {
                        var result = OnReceivedHandle(content);
                        if (!result.Result)
                        {
                            _logger.LogError($"kafka consumer Error：topic:{consume.Topic},key:{consume.Message.Key},content:{content}");
                        }
                        consumer.Commit(consume); //提交消费确认
                    }

                    if (queueLog)
                    {
                        _logger.LogInformation($"kafka listener：topic:{consume.Topic},key:{consume.Message.Key},content:{content}");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"kafka订阅失败，error:{e.Message}");
            }
            finally
            {
                consumer?.Dispose();
            }
        }

        private IConsumer<string, string> CreateConsumerBuilder()
        {
            var config = new ConsumerConfig
            {
                GroupId = GroupId,
                BootstrapServers = _options.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false // 设置非自动偏移，业务逻辑完成后手动处理偏移，防止数据丢失
            };

            if (!string.IsNullOrEmpty(_options.SaslUsername))
            {
                config.SaslUsername = _options.SaslUsername;
                config.SaslPassword = _options.SaslPassword;
                config.SaslMechanism = SaslMechanism.Plain;
                config.SecurityProtocol = SecurityProtocol.SaslPlaintext;
            }
            var consumer = new ConsumerBuilder<string, string>(config)
                   .SetErrorHandler((_, e) => _logger.LogError($"Kafka listener Error: {e.Reason}"))
                   .Build();

            return new ConsumerBuilder<string, string>(config).Build();
        }
    }
}
