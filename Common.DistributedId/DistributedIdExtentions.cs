using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
namespace Common.DistributedId
{
    /// <summary>
    /// extensions
    /// </summary>
    public static class DistributedIdExtentions
    {
        public static IHostBuilder ConfigureDistirbutedIdDefaults(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((host, services) =>
            {
                var option = host.Configuration.GetChildren()
                   .Where(x => x.Key.ToLower() == "distributedid")
                   .FirstOrDefault()?.Get<DistributedIdOptions>() ?? new DistributedIdOptions();

                services.AddDistributedId(option);
            });
            return hostBuilder;
        }

        /// <summary>
        /// 注入分布式Id
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="distributedIdOption"></param>
        /// <returns></returns>
        public static IServiceCollection AddDistributedId(this IServiceCollection services, DistributedIdOptions distributedIdOption)
        {
            services.AddOptions<DistributedIdOptions>().Configure(x =>
            {
                x.GetType().GetProperties().ToList().ForEach(aProperty =>
                {
                    aProperty.SetValue(x, aProperty.GetValue(distributedIdOption));
                });
            });

            return services.AddSingleton<IDistributedId, DistributedId>();
        }
    }
}
