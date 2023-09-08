using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MongoDb
{
    public static class MongoDbServiceCoolectionExtensions
    {
        /// <summary>
        /// 添加MongoDb
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section"></param>
        public static void AddMongoDb(this IServiceCollection services, IConfigurationSection section)
        {
            services.Configure<MongoDbOptions>(options =>
            {
                options.Connection = section.GetSection("ConnectionString").Value??throw new ArgumentNullException("mongodb connection isnull");
                options.DataBase = section.GetSection("DataBase").Value ?? throw new ArgumentNullException("mongodb database isnull");
            });
            services.AddScoped<MongoDbService>();
        }
        // 获取MongoDbOptions配置节
        //IConfigurationSection section = configuration.GetSection("MongoDbOptions");
    }
}
