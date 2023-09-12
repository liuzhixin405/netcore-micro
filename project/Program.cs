using System.Reflection;
using Cache;
using Cache.Options;
using DistributedId;
using DistributedLock.Abstractions;
using Microsoft.AspNetCore.Mvc;
using project.Extensions;
using project.Filters;
using project.Models;
using project.SeedWork;
using WatchDog;
namespace project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidFilter>();
                options.Filters.Add<GlobalExceptionFilter>();
            });
            builder.AddDatabase();
            #region 雪花id 分布式
            builder.Services.AddDistributedLock(x =>
            {
                x.LockType = LockType.InMemory;
                x.RedisEndPoints = new string[] { builder.Configuration["DistributedRedis:ConnectionString"] ?? throw new Exception("$未能获取distributedredis连接字符串")};
            }).AddCache(new CacheOptions
            {
                CacheType = CacheTypes.Redis,
                RedisConnectionString = builder.Configuration["DistributedRedis:ConnectionString"] ?? throw new Exception("$未能获取distributedredis连接字符串")
            }).AddDistributedId(new DistributedIdOptions
            {
                Distributed = true
            });
            #endregion 
            #region automapper
            builder.Services.AddAutoMapper(new Assembly[] { typeof(ProductProfile).Assembly, typeof(CustomerProfile).Assembly });
            #endregion
            #region redis
            builder.AddRedis();
            #endregion
            #region mqsetting
            builder.AddMq();
            #endregion
            #region essearch
            builder.AddEsSearch();
            #endregion
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerDocument();
            #region 添加看门狗
            builder.AddWatchDog();
            #endregion
            //builder.Services.AddProblemDetails();
            var app = builder.Build();
            DatabaseStartup.CreateTable(app.Services);
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment()) //docker测试
            //{
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseDeveloperExceptionPage();
            //}
            #region usewatchdog
            app.UseWatchDog(builder.Configuration);
            #endregion
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}