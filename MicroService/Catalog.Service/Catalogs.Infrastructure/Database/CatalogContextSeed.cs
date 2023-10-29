using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogs.Domain.Catalogs;
using DistributedId;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Catalogs.Infrastructure.Database
{
    public class CatalogContextSeed
    {
        public async Task SeedAsync(CatalogContext context,ILogger<CatalogContextSeed> logger,IServiceProvider service) 
        {
            var s = new string[] { "1", "2" };
            var policy = CreatePolicy(logger, nameof(CatalogContextSeed));
            using var scope = service.CreateAsyncScope();
            var distributedId = scope.ServiceProvider.GetService<IDistributedId>()??throw new ArgumentNullException(nameof(IDistributedId));
            await policy.ExecuteAsync(async () =>
            {
                await context.Catalogs.AddRangeAsync(new Catalogs.Domain.Catalogs.Catalog[] {
                    Catalog.CreateNew(distributedId.NewLongId(), "iphone16", 7000, 1000, 5000, "热销产品1"),
                    Catalog.CreateNew(distributedId.NewLongId(), "小米15", 7000, 1000, 5000, "热销产品2"),
                    Catalog.CreateNew(distributedId.NewLongId(), "荣耀1", 7000, 1000, 5000, "热销产品3"),
                    Catalog.CreateNew(distributedId.NewLongId(), "中兴天机", 7000, 1000, 5000, "热销产品4")
                            });
                await context.SaveChangesAsync();
            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<CatalogContextSeed> logger,string prefix,int retries = 3)
        {
            return Policy.Handle<SqlException>().WaitAndRetryAsync(retryCount:retries,sleepDurationProvider:retries=>TimeSpan.FromSeconds(5),
                onRetry: (exception, timespance, retry, ctx) =>
                {
                    logger.LogWarning(exception, "[{prefix}] Error seeding database (attempt {retry} of {retries})", prefix, retry, retries);
                });
        }
    }
}
