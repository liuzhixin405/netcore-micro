using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogs.Domain.Catalogs;
using Common.DistributedId;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Catalogs.Infrastructure.Database
{
    public class CatalogContextSeed
    {
        private readonly IServiceProvider _serviceProvider;
        public CatalogContextSeed(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task SeedAsync() 
        {
            IServiceProvider service = _serviceProvider;
            using var scope = service.CreateAsyncScope();
            var logger = scope.ServiceProvider.GetService<ILogger<CatalogContextSeed>>();
            var context = scope.ServiceProvider.GetService<CatalogContext>();
            var s = new string[] { "1", "2" };
            var policy = CreatePolicy(logger, nameof(CatalogContextSeed));
            if ((await context.Catalogs.LongCountAsync()) > 0)
                return;
            var distributedId = scope.ServiceProvider.GetService<IDistributedId>()??throw new ArgumentNullException(nameof(IDistributedId));
            await policy.ExecuteAsync(async () =>
            {
                await context.Catalogs.AddRangeAsync(new Catalogs.Domain.Catalogs.Catalog[] {
                    Catalog.CreateNew(distributedId.NewLongId(), "iphone16",decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "小米15", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "荣耀1", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "太阳伞", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "茶杯", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "口罩", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "龙鳞", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "马鞍", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "天之骄子", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "百里守约", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "乌龟", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "矿泉水", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "米饭", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "无相神功", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "美团", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "三里屯", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "其几天八五", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "刮胡刀", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "我的日子", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")),Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "你是谁", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10)),
                    Catalog.CreateNew(distributedId.NewLongId(), "我的天", decimal.Parse((Random.Shared.NextDouble()*1000).ToString("F2")), Random.Shared.Next(1,1000), Random.Shared.Next(1001,5000), "热销产品"+Random.Shared.Next(1,10))

                            });
                await context.SaveChangesAsync();
            });
        }

        private static AsyncRetryPolicy CreatePolicy(ILogger<CatalogContextSeed> logger,string prefix,int retries = 3)
        {
            return Policy.Handle<SqlException>().WaitAndRetryAsync(retryCount:retries,sleepDurationProvider:retries=>TimeSpan.FromSeconds(5),
                onRetry: (exception, timespance, retry, ctx) =>
                {
                    logger.LogWarning(exception, "[{prefix}] Error seeding database (attempt {retry} of {retries})", prefix, retry, retries);
                });
        }
    }
}
