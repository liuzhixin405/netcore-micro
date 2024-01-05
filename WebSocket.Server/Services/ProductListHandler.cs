using System.Net.WebSockets;
using System.Threading.Tasks;
using System;
using WsServer.Handler;
using WsServer.Manager;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Catalogs.Domain.Catalogs;
using Catalogs.Domain.Dtos;
using System.Net.Sockets;

namespace WebScoket.Server.Services
{
    /// <summary>
    /// 实时推送产品主要是最新的库存，其他信息也会更新
    /// </summary>
    public class ProductListHandler : WebSocketHandler
    {
        private System.Threading.Timer _timer;
        private readonly IDatabase _redisDb;
        //展示列表推送
        private string productIdsStr;
        public ProductListHandler(WebSocketConnectionManager webSocketConnectionManager,IConfiguration configuration) : base(webSocketConnectionManager)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configuration["DistributedRedis:ConnectionString"] ?? throw new Exception("$未能获取distributedredis连接字符串"));
            _redisDb = redis.GetDatabase();
            _timer = new System.Threading.Timer(Send, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }
        private void Send(object state)
        {
            // 获取当前时间并发送给所有连接的客户端
            if (productIdsStr != null)
            {
                string[] productIds = System.Text.Json.JsonSerializer.Deserialize<string[]>(productIdsStr);
                string hashKeyToRetrieve = "products";
                List<ProductDto> products = new List<ProductDto>();

                foreach (var productId in productIds)
                {
                    if(productId == "null") {
                        continue;
                    }
                    string retrievedProductValue = _redisDb.HashGet(hashKeyToRetrieve, productId);
                    if (!string.IsNullOrEmpty(retrievedProductValue))
                    {
                        //反序列化和构造函数冲突，改造了一下Catalog
                        Catalog catalog = System.Text.Json.JsonSerializer.Deserialize<Catalog>(retrievedProductValue);
                        products.Add(new ProductDto(catalog.Id.ToString(), catalog.Name, catalog.Price.ToString(), catalog.Stock.ToString(), catalog.ImgPath));
                    }
                }
                if (products.Count > 0)
                {
                     SendMessageToAllAsync(System.Text.Json.JsonSerializer.Serialize(products)).Wait();
                }
                else
                {
                    SendMessageToAllAsync("NoProduct").Wait();
                }
            }
        }
        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            //每次页面有刷新就会拿到展示的id列表
            productIdsStr = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
        }
    }
}
