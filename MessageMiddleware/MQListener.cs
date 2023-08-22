using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using MessageMiddleware.Factory;
using MessageMiddleware.Kafka.Consumer;
using MessageMiddleware.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageMiddleware
{
    /// <summary>
    /// MQ 监听服务
    /// </summary>
    public abstract class MQListener : BackgroundService
    {
        /// <summary>
        /// 队列名称，如果是kafka 则是topic
        /// </summary>
        protected string Queue { get; set; }

        /// <summary>
        /// 交换机
        /// </summary>
        protected string Exchange { get; set; } = string.Empty;

        /// <summary>
        /// 路由key
        /// </summary>
        protected string RoutingKey { get; set; } = "#";

        private Func<Task> _stopAction;

        private IServiceProvider Services { get; set; }

        protected readonly MQConfig _config;

        public MQListener(IServiceProvider services)
        {
            Services = services;
            _config = services.GetService<MQConfig>();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Init();
            CreateListener(_config);
            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _stopAction?.Invoke();
            await base.StopAsync(cancellationToken);
        }

        public void CreateListener(MQConfig config)
        {
            var mqType = (MQType)Enum.Parse(typeof(MQType), config.Use.ToString());
            switch (mqType)
            {
                case MQType.Rabbit:
                    var rabbit = new MQListenerHostService(Services)
                    {
                        Config = new ListenerConfig
                        {
                            Queue = Queue,
                            Exchange = Exchange,
                            RoutingKey = RoutingKey,
                            OnReceivedHandle = Process
                        }                     
                    };
                    _stopAction = rabbit.Stop;
                    rabbit.Consumer(_config.ConsumerLog);
                    break;
                case MQType.Kafka:
                    var kafka = new KafkaListenerHostService(Services)
                    {
                        Queue = Queue,
                        OnReceivedHandle = Process
                    };
                    kafka.Consumer();
                    break;
                default:
                    throw new ArgumentException($"Not Support Servivce，{nameof(CreateListener)}");
            }
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="message">订阅的消息</param>
        /// <returns></returns>
        public abstract Task<bool> Process(string message);

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public abstract bool Init();

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
