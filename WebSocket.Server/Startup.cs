using Cache.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using WebScoket.Server.Services;
using WsServer.Extensions;
using WsServer.Handler;
using WsServer.Manager;
using WsServer.Middleware;
using WsServer.Services;
using Cache;
public class Startup
{
    private readonly IConfiguration _configuration;
    public Startup(IConfiguration configuration)
    {
            _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddWebSocketManager();
        #region 雪花id 分布式
        services.AddCache(new CacheOptions
        {
            CacheType = CacheTypes.Redis,
            RedisConnectionString = _configuration["DistributedRedis:ConnectionString"] ?? throw new Exception("$未能获取distributedredis连接字符串")
        });
        #endregion
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseWebSockets();
        app.MapWebSocketManager("/time", new TimeHandler(new WebSocketConnectionManager() ));
        app.MapWebSocketManager("/productlist", new ProductListHandler(new WebSocketConnectionManager(), _configuration));
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.Equals("/"))
            {
               await context.Response.WriteAsync("websocket is running");
            }
            else
            {
                await next(context);
            }
        });
    }
}




