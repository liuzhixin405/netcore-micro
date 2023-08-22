using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using MessageMiddleware.Kafka.Producers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageMiddleware.Kafka
{
    /// <summary>
    /// kafka扩展
    /// </summary>
    public static class KafkaExtensions
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddKafka(this IServiceCollection services, Action<ProducerOptions> configure)
        {
            if (configure == null) throw new ArgumentNullException($"{nameof(configure)} is null");
            services.AddSingleton<IKafkaProduce, KafkaProduce>();
            services.Configure(configure);
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddKafka(this IServiceCollection services, ProducerOptions Kafka)
        {
            var configure = services.BuildServiceProvider().GetService<IConfiguration>();
            services.AddSingleton<IKafkaProduce, KafkaProduce>();
            services.AddSingleton(Kafka);
            return services;
        }

        internal static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
