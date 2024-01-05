using System.Composition.Convention;
using System.Diagnostics;
using System.Reflection;
using System.Threading.RateLimiting;
using Common.Cache;
using DistributedId;
using DistributedLock.Abstractions;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using project.Extensions;
using project.Filters;
using project.Models;
using project.Options;
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
#if DEBUG
            builder.Services.AddW3CLogging(logging =>
            {
                // Log all W3C fields
                logging.LoggingFields = W3CLoggingFields.All;

                logging.AdditionalRequestHeaders.Add("x-forwarded-for");
                logging.AdditionalRequestHeaders.Add("x-client-ssl-protocol");
                logging.FileSizeLimit = 5 * 1024 * 1024;
                logging.RetainedFileCountLimit = 2;
                logging.FileName = "MyLogFile";
                logging.LogDirectory = @"D:\logs";
                logging.FlushInterval = TimeSpan.FromSeconds(2);
            });
#endif
            #region 限速
            var slipExpRatOptions = new SlipExpirationRateLimitOptions();
            builder.Configuration.GetSection(SlipExpirationRateLimitOptions.SlipExpirationRateLimit).Bind(slipExpRatOptions);
            builder.Services.AddRateLimiter(_ => _.AddSlidingWindowLimiter("fixed", options =>
            {
                options.PermitLimit = slipExpRatOptions.PermitLimit;
                options.Window = TimeSpan.FromSeconds(slipExpRatOptions.Window);
                options.SegmentsPerWindow = slipExpRatOptions.SegmentsPerWindow;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = slipExpRatOptions.QueueLimit;
            })); //30秒最多3000个请求
            #endregion
            var app = builder.Build();
           
            DatabaseStartup.CreateTable(app.Services);
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment()) //docker测试
            //{
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseDeveloperExceptionPage();
            //}
#if DEBUG
            #region w3clog
            app.UseW3CLogging();
            #endregion
#endif
            #region usewatchdog
            app.UseWatchDog(builder.Configuration);           
            app.UseRateLimiter();
            #endregion
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}