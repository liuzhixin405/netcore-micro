using project.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace project.SeedWork
{
    public static class DatabaseStartup
    {
        public static void CreateTable(IServiceProvider service)
        {
            using var scope = service.CreateScope();
            var context = scope.ServiceProvider.GetService<ReadProductDbContext>();
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
