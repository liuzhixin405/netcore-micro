using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WsServer.Handler;
using WsServer.Manager;
using WsServer.Middleware;

namespace WsServer.Extensions
{
    public static class WebSocketExtensions
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
            string path, WebSocketHandler handler)
        {
            return app.Map(path, (x) => x.UseMiddleware<WebSocketManagerMiddleware>(handler));
        }

        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<WebSocketConnectionManager>();
            return services;
        }
    }
}
