using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using WsServer.Extensions;
using WsServer.Handler;
using WsServer.Manager;
using WsServer.Middleware;
using WsServer.Services;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddWebSocketManager();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseWebSockets();
        app.MapWebSocketManager("/time", new TimeHandler(new WebSocketConnectionManager() ));
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




