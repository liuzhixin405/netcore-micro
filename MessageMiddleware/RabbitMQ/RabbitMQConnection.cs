using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MessageMiddleware.RabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly RabbitMQSetting _setting;
        private readonly ILogger<RabbitMQConnection> _logger;

        private IConnection _connection;

        public RabbitMQConnection(RabbitMQSetting setting, ILogger<RabbitMQConnection> logger)
        {
            _logger = logger;
            _setting = setting;
        }

        public IConnection GetConnection()
        {
            return _connection ??= CreateConnection();
        }

        private IConnection CreateConnection()
        {
            var ampList = new List<AmqpTcpEndpoint>();

            foreach (var connect in _setting.ConnectionString)
            {
                var amp = new AmqpTcpEndpoint(new Uri(connect));
                if (_setting.SslEnabled)
                {
                    amp.Ssl = new SslOption() { Enabled = true, AcceptablePolicyErrors = System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch };
                }
                ampList.Add(amp);
            }
             
            var factory = new ConnectionFactory()
            {
                UserName = _setting.UserName,
                Password = _setting.Password,
                Port = _setting.Port,
                //TopologyRecoveryEnabled = true,
                //AutomaticRecoveryEnabled = true, 
                RequestedHeartbeat = TimeSpan.FromSeconds(300),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(20),
                //DispatchConsumersAsync =true, // 异步
                RequestedConnectionTimeout =  TimeSpan.FromSeconds(20),
            };

            var connection = factory.CreateConnection(ampList);
            connection.ConnectionShutdown += (sender, e) => { _logger.LogWarning($"RabbitMQ client connection closed! --> {e.ReplyText}"); };
            connection.ConnectionBlocked += (sender, e) => _logger.LogWarning($"RabbitMQ client connection blocked! --> {e.Reason}");
            connection.CallbackException += (sender, e) => { _logger.LogError($"RabbitMQ client connection exception! --> {e?.Exception?.Message}"); };
            _logger.LogInformation($"RabbitMQ client connection open!--> {connection.Endpoint} ");
            return connection;
        }


    }
}
