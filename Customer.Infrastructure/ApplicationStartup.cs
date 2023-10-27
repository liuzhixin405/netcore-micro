using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Customers.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Customers.Infrastructure
{
    public static class ApplicationStartup
    {
        public static IServiceCollection AddDB(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<CustomerContext>(options=>
            {
                options.UseSqlServer(configuration["ConnectionString"]);
            },ServiceLifetime.Scoped);
            
            return services;
        }

        public static void CreateTable(IServiceProvider service)
        {
            using var scope = service.CreateScope();
            var context = scope.ServiceProvider.GetService<CustomerContext>();
            if (context.Database.ProviderName.Equals("Microsoft.EntityFrameworkCore.InMemory"))
            {
                return;
            }
            if (!(context.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists()) //数据库不存在自动创建，并建表
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }
}
