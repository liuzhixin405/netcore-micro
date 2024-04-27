using Common.Util.Exceptions;
using MessageMiddleware.Factory;
using MessageMiddleware.RabbitMQ;

namespace project.Extensions
{
    public static partial class TheExtensions
    {
        public static void AddMq(this WebApplicationBuilder builder)
        {
            try
            {
                var rabbitMqSetting = new RabbitMQSetting
                {
                    ConnectionString = builder.Configuration["MqSetting:RabbitMq:ConnectionString"].Split(';'),
                    Password = builder.Configuration["MqSetting:RabbitMq:PassWord"],
                    Port = int.Parse(builder.Configuration["MqSetting:RabbitMq:Port"]),
                    SslEnabled = bool.Parse(builder.Configuration["MqSetting:RabbitMq:SslEnabled"]),
                    UserName = builder.Configuration["MqSetting:RabbitMq:UserName"],
                };
                var kafkaSetting = new MessageMiddleware.Kafka.Producers.ProducerOptions
                {
                    BootstrapServers = builder.Configuration["MqSetting:Kafka:BootstrapServers"],
                    SaslUsername = builder.Configuration["MqSetting:Kafka:SaslUserName"],
                    SaslPassword = builder.Configuration["MqSetting:Kafka:SaslPassWord"],
                    Key = builder.Configuration["MqSetting:Kafka:Key"]
                };
                var mqConfig = new MQConfig
                {
                    ConsumerLog = bool.Parse(builder.Configuration["MqSetting:ConsumerLog"]),
                    PublishLog = bool.Parse(builder.Configuration["MqSetting:PublishLog"]),
                    Rabbit = rabbitMqSetting,
                    Use = int.Parse(builder.Configuration["MqSetting:Use"]),
                    Kafka = kafkaSetting
                };

                builder.Services.AddSingleton<MQConfig>(sp => mqConfig);
                builder.Services.AddMQ(mqConfig);
            }
            catch (Exception ex)
            {
                throw new SystemErrorException($"消息中间件配置有误:{ex.Message}");
            }
           
        
        }
    }
}
