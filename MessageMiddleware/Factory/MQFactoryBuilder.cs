using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MessageMiddleware.Kafka;
using MessageMiddleware.Kafka.Producers;
using MessageMiddleware.RabbitMQ;
using System;
using Microsoft.Extensions.ObjectPool;


namespace MessageMiddleware.Factory
{
    public static class MQFactoryBuilder
    {
        /// <summary>
        /// init mq
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddMQ(this IServiceCollection services,MQConfig config)
        {
            #region register
            services.AddSingleton<KafkaPublisher>();
            services.AddSingleton<MQPublisher>();
            services.AddSingleton<IMQFactory, MQFactory>(); //注入工厂
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton(factory =>
            {
                Func<string, IMQPublisher> impl = key =>
                {
                    if (key.Equals(MQType.Kafka.ToString()))
                    {
                        return factory.GetService<KafkaPublisher>();
                    }
                    else if (key.Equals(MQType.Rabbit.ToString()))
                    {
                        return factory.GetService<MQPublisher>();
                    }
                    else
                    {
                        throw new ArgumentException($"Not Support key : {key}");
                    }
                };
                return impl;
            });
            #endregion

            services.AddSingleton(config);

            //注入rabbit
            services.UseRabbitMQ(() => { return config.Rabbit; });
            //注入kafka
            services.AddKafka(config.Kafka);

            CreateBuilder(services, config);
            return services;
        }

        private static IServiceCollection CreateBuilder(IServiceCollection services, MQConfig config)
        {
            var mqType = (MQType)Enum.Parse(typeof(MQType), config.Use.ToString());
            switch (mqType)
            {
                case MQType.Rabbit:
                    services.AddSingleton<IMQPublisher, MQPublisher>();
                    break;
                case MQType.Kafka:
                    services.AddSingleton<IMQPublisher, KafkaPublisher>();
                    break;
                default:
                    throw new ArgumentException($"Not Support Servivce，{nameof(CreateBuilder)}");
            }
            return services;
        }
    }
}
