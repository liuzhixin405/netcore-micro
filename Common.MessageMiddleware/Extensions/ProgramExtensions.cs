using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageMiddleware.Factory;
using MessageMiddleware.Kafka.Producers;
using MessageMiddleware.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.MessageMiddleware.Extensions
{
    public static partial class ProgramExtensions
    {
        public static void AddMq(this IServiceCollection sc, IConfiguration configuration)
        {
            var rabbitMqSetting = new RabbitMQSetting
            {
                ConnectionString = configuration["MqSetting:RabbitMq:ConnectionString"].Split(';'),
                Password = configuration["MqSetting:RabbitMq:PassWord"],
                Port = int.Parse(configuration["MqSetting:RabbitMq:Port"]),
                SslEnabled = bool.Parse(configuration["MqSetting:RabbitMq:SslEnabled"]),
                UserName = configuration["MqSetting:RabbitMq:UserName"],
            };
            var kafkaSetting = new ProducerOptions
            {
                BootstrapServers = configuration["MqSetting:Kafka:BootstrapServers"],
                SaslUsername = configuration["MqSetting:Kafka:SaslUserName"],
                SaslPassword = configuration["MqSetting:Kafka:SaslPassWord"],
                Key = configuration["MqSetting:Kafka:Key"]
            };
            var mqConfig = new MQConfig
            {
                ConsumerLog = bool.Parse(configuration["MqSetting:ConsumerLog"]),
                PublishLog = bool.Parse(configuration["MqSetting:PublishLog"]),
                Rabbit = rabbitMqSetting,
                Use = int.Parse(configuration["MqSetting:Use"]),
                Kafka = kafkaSetting
            };
            sc.AddSingleton<MQConfig>(sp => mqConfig);
            sc.AddMQ(mqConfig);
        }
    }
}
