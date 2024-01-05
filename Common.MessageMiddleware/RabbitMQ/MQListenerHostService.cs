using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageMiddleware.RabbitMQ
{
    /// <summary>
    /// Rabbit 服务
    /// </summary>
    public class MQListenerHostService
    {
        public ListenerConfig Config { get; set; }
        private readonly DefaultObjectPool<IModel> _channelPool;
        private readonly ILogger<MQListenerHostService> _logger;
        private Action _stopAction;
        private bool _stoped;
        private readonly TaskCompletionSource tcs;

        public MQListenerHostService(IServiceProvider services)
        {
            _logger = services.GetRequiredService<ILogger<MQListenerHostService>>();
            var mqConnection = services.GetRequiredService<IRabbitMQConnection>();
            _channelPool = new DefaultObjectPool<IModel>(new RabbitMQChannelPooledObjectPolicy(mqConnection, _logger), 3);
            tcs = new TaskCompletionSource();
        }

        public void Consumer(bool queueLog=true)
        {
            var channel = _channelPool.Get();
            try
            {
                channel.QueueDeclare(Config.Queue, true, false, false, null);
                if (!string.IsNullOrWhiteSpace(Config.Exchange))
                {
                    channel.QueueBind(Config.Queue, Config.Exchange, Config.RoutingKey);
                }
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    try
                    {
                       // _logger.LogInformation(ea.DeliveryTag + ", " + ea.ConsumerTag + ": " + ea.Body.Length);
                        var body = ea.Body.Span;
                        var message = Encoding.UTF8.GetString(body);
                        if (Config.OnReceivedHandle != null)
                        {
                            if (queueLog)
                            {
                                _logger.LogInformation($"rabbit 收到消息，开始消费逻辑：queue:{Config.Queue},exchange:{ea.Exchange},routingKey:{ea.RoutingKey},message:{message}");
                            }
                           
                            var result = Config.OnReceivedHandle(message);
                            if (!result.Result)
                            {
                                _logger.LogError($"rabbit consumer Error：queue:{Config.Queue},exchange:{ea.Exchange},routingKey:{ea.RoutingKey},message:{message}");
                            } 
                        }

                    }
                    catch(Exception ex)
                    {
                        _logger.LogError($"rabbit receive error:{ex.Message}");
                    }
                    finally
                    {
                        channel.BasicAck(ea.DeliveryTag, false);
                        //_logger.LogInformation("rabbit message ack success");
                        if (_stoped)
                        {
                            tcs.SetResult();
                            Thread.Sleep(500);//休眠500ms 等待mq关闭
                        }
                    }
         
                };
                consumer.Shutdown += (o, e) =>
                {
                    _logger.LogError($"rabbit consumer shutdown：message:{e}");
                };
                
                channel.BasicConsume(Config.Queue, false, consumer);
                _stopAction = channel.Close;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"rabbitMQ订阅失败，queue:{Config.Queue},error:{e.Message}");
            }
            finally
            {
                _channelPool.Return(channel);
            }
            channel.CallbackException += (sender, e) => 
            {
                _logger.LogError($"rabbit model callbackException,error:{e?.Exception?.Message}");
            };
           // _logger.LogInformation($"rabbit consumer complete，queue:{Config.Queue}");
        }


        public async Task Stop()
        {
            _stoped = true;
            await tcs.Task;
            _stopAction?.Invoke();
            _logger.LogWarning("已停止监听rabbitMq");
        }
    }
}
