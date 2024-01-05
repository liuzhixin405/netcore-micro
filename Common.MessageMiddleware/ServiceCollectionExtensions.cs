using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MessageMiddleware.RabbitMQ;
using System; 

namespace MessageMiddleware
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseRabbitMQ(this IServiceCollection services, Func<RabbitMQSetting> setting)
        {
            services.AddSingleton(setting.Invoke());
            services.AddSingleton<IRabbitMQConnection, RabbitMQConnection>();

            services.AddSingleton<IMQPublisher, MQPublisher>();

            return services;
        }
    }
}
