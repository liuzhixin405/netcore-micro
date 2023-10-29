using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MicroService.Shared.GrpcPool
{
    public class GrpcClientPool<TClient>
    {
        private readonly static ConcurrentBag<TClient> _clientPool = new ConcurrentBag<TClient>();
       
        private readonly IGrpcClientFactory<TClient> _clientFactory;
      
        private readonly int _maxConnections;
        private readonly TimeSpan _handoverTimeout;
        private readonly string _address;
        private readonly DateTime _now;
        public GrpcClientPool(IGrpcClientFactory<TClient> clientFactory,
            IConfiguration configuration,string address)
        {
            _now =  DateTime.Now;
            _clientFactory = clientFactory;
            _maxConnections = int.Parse(configuration["Grpc:maxConnections"]?? throw new ArgumentNullException("grpc maxconnections is null"));
            _handoverTimeout = TimeSpan.FromSeconds(double.Parse(configuration["Grpc:maxConnections"]??throw new ArgumentNullException("grpc timeout is null")));
            _address = address;
        }

        public TClient GetClient()
        {
            if (_clientPool.TryTake(out var client))
            {
                return client;
            }

            if (_clientPool.Count < _maxConnections)
            {
                var channel = GrpcChannel.ForAddress(_address);
                client = _clientFactory.Create(channel);
                _clientPool.Add(client);
                return client;
            }

            if (!_clientPool.TryTake(out client) && DateTime.Now.Subtract(_now) > _handoverTimeout)
            {
                throw new TimeoutException("Failed to acquire a connection from the pool within the specified timeout.");
            }
            return client;
        }

        public void ReleaseClient(TClient client)
        {
            if (client == null)
            {
                return;
            }
            _clientPool.Add(client);
        }
    }
}
