using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Database;
using Ordering.Infrastructure.Repositories;
using project.Repositories;
using RepositoryComponent.DbFactories;

namespace Ordering.Infrastructure.Extensions
{
    public static partial class OrderExtensions
    {
        public static void AddDatabase(this IServiceCollection sc,IConfiguration configuration)
        {
            
            ///sqlserver   
            if (configuration["DbType"]?.ToLower() == "sqlserver")
            {
                sc.AddDbContext<ReadOrderDbContext>(options => options.UseSqlServer(configuration["ConnectionStrings:SqlServer:ReadConnection"]), ServiceLifetime.Scoped);
                sc.AddDbContext<WriteOrderDbContext>(options => options.UseSqlServer(configuration["ConnectionStrings:SqlServer:WriteConnection"]), ServiceLifetime.Scoped);

            }
            ///mysql
            else if (configuration["DbType"]?.ToLower() == "mysql")
            {
                sc.AddDbContext<ReadOrderDbContext>(options => options.UseMySQL(configuration["ConnectionStrings:MySql:ReadConnection"]), ServiceLifetime.Scoped);
                sc.AddDbContext<WriteOrderDbContext>(options => options.UseMySQL(configuration["ConnectionStrings:MySql:WriteConnection"]), ServiceLifetime.Scoped);

            }
            else
            {
               
                sc.AddDbContext<ReadOrderDbContext>(options => options.UseInMemoryDatabase("test_inmemory_db"), ServiceLifetime.Scoped);
                sc.AddDbContext<WriteOrderDbContext>(options => options.UseInMemoryDatabase("test_inmemory_db"), ServiceLifetime.Scoped);

            }

            sc.AddScoped<Func<ReadOrderDbContext>>(provider => () => provider.GetService<ReadOrderDbContext>() ?? throw new ArgumentNullException("ReadOrderDbContext is not inject to program"));
            sc.AddScoped<Func<WriteOrderDbContext>>(provider => () => provider.GetService<WriteOrderDbContext>() ?? throw new ArgumentNullException("WriteOrderDbContext is not inject to program"));

            sc.AddScoped<DbFactory<WriteOrderDbContext>>();
            sc.AddScoped<DbFactory<ReadOrderDbContext>>();

            sc.AddTransient<IReadOrderRepository, OrderReadRepository>();
            sc.AddTransient<IWriteOrderRepository, OrderWriteRepository>();
        }
    }
}
