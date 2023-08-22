using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using System; 

namespace MessageMiddleware.RabbitMQ
{
    public class RabbitMQChannelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly IRabbitMQConnection _connection;
        private readonly ILogger _logger;

        public RabbitMQChannelPooledObjectPolicy(IRabbitMQConnection connection, ILogger logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public IModel Create()
        {
            try
            {
                return _connection.GetConnection().CreateModel();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "RabbitMQ Create Model Fail!");
                throw;
            }
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }

            obj?.Dispose();
            return false;
        }
    }
}