using RabbitMQ.Client; 

namespace MessageMiddleware.RabbitMQ
{
    public interface IRabbitMQConnection
    {
        IConnection GetConnection();
    }
}
