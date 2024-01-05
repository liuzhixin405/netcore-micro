using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageMiddleware.Kafka.Producers
{
    public interface IKafkaProduce
    {
        Task PublishAsync<TMessage>(string topic, TMessage message) where TMessage : class;
        Task PublishAsync<TMessage>(string topic,string key, TMessage message) where TMessage : class;

        Task PublishAsync(string topic, string message);

    }
}
