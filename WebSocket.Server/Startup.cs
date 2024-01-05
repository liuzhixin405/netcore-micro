using Common.Cache;
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
using Microsoft.Extensions.Logging;
using Common.Redis.Extensions;
using Common.Redis.Extensions.Configuration;
using Common.Redis.Extensions.Serializer;

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
      
        //redis
        services.AddSingleton<IRedisCache>(obj =>
        {
            var config = _configuration.GetSection("Redis").Get<RedisConfiguration>();
            var serializer = new MsgPackSerializer();
            var connection = new PooledConnectionMultiplexer(config.ConfigurationOptions);
            return new RedisCache(obj.GetService<ILoggerFactory>().CreateLogger<RedisCache>(), connection, config, serializer);
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseWebSockets();
        app.MapWebSocketManager("/time", new TimeHandler(new WebSocketConnectionManager() ));
        app.MapWebSocketManager("/productlist", new ProductListHandler(new WebSocketConnectionManager(),app.ApplicationServices));
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




