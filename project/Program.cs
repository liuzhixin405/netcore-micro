using project.Context;
using project.Models;
using project.Repositories;
using project.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using project.Repositories;
using project.SeedWork;
using RepositoryComponent.DbFactories;
using Microsoft.AspNetCore.Builder;
using project.Filters;
using Microsoft.AspNetCore.Mvc;
using project.Utility.Helper;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static System.Net.Mime.MediaTypeNames;
using MessageMiddleware.Factory;
using MessageMiddleware.RabbitMQ;
using Redis.Extensions.Serializer;
using Redis.Extensions;
using System.Configuration;
using Redis.Extensions.Configuration;

namespace project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<ApiBehaviorOptions>(options=>options.SuppressModelStateInvalidFilter = true);
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidFilter>();
                options.Filters.Add<GlobalExceptionFilter>();
            });
            ///sqlserver   
                if (builder.Configuration["DbType"]?.ToLower() == "sqlserver")
            {
                builder.Services.AddDbContext<ReadProductDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:SqlServer:ReadConnection"]), ServiceLifetime.Scoped);
                builder.Services.AddDbContext<WriteProductDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:SqlServer:WriteConnection"]), ServiceLifetime.Scoped);

            }
            ///mysql
            else if (builder.Configuration["DbType"]?.ToLower() == "mysql")
            {
                builder.Services.AddDbContext<ReadProductDbContext>(options => options.UseMySQL(builder.Configuration["ConnectionStrings:MySql:ReadConnection"]), ServiceLifetime.Scoped);
                builder.Services.AddDbContext<WriteProductDbContext>(options => options.UseMySQL(builder.Configuration["ConnectionStrings:MySql:WriteConnection"]), ServiceLifetime.Scoped);

            }
            else
            {
                //throw new ArgumentNullException("δ����ȷ��ע�����ݿ�");
                builder.Services.AddDbContext<ReadProductDbContext>(options => options.UseInMemoryDatabase("test_inmemory_db"), ServiceLifetime.Scoped);
                builder.Services.AddDbContext<WriteProductDbContext>(options => options.UseInMemoryDatabase("test_inmemory_db"), ServiceLifetime.Scoped);

            }

            builder.Services.AddScoped<Func<ReadProductDbContext>>(provider => () =>provider.GetService<ReadProductDbContext>()??throw new ArgumentNullException("ReadProductDbContext is not inject to program"));
            builder.Services.AddScoped<Func<WriteProductDbContext>>(provider => () => provider.GetService<WriteProductDbContext>() ?? throw new ArgumentNullException("WriteProductDbContext is not inject to program"));

            builder.Services.AddScoped<DbFactory<WriteProductDbContext>>();
            builder.Services.AddScoped<DbFactory<ReadProductDbContext>>();

            builder.Services.AddTransient<IReadProductRepository,ProductReadRepository>();
            builder.Services.AddTransient<IWriteProductRepository, ProductWriteRepository>();
            builder.Services.AddTransient<IProductService, ProductService>();

            builder.Services.AddTransient<ICustomerService,CustomerService>();

            #region redis
            CacheHelper.Init(builder.Configuration); //跟下面的差不多

            builder.Services.AddSingleton<IProductRedis>(obj =>
            {
                var config = builder.Configuration.GetSection("ProductRedis").Get<RedisConfiguration>();
                var serializer = new MsgPackSerializer();
                var connection = new PooledConnectionMultiplexer(config.ConfigurationOptions);
                return new ProductRedis(obj.GetService<ILoggerFactory>().CreateLogger<ProductRedis>(), connection, config, serializer);
            });
            #endregion

            #region rabbitmqsetting
            var rabbitMqSetting = new RabbitMQSetting
            {
                ConnectionString = builder.Configuration["MqSetting:RabbitMq:ConnectionString"].Split(';'),
                Password = builder.Configuration["MqSetting:RabbitMq:PassWord"],
                Port = int.Parse(builder.Configuration["MqSetting:RabbitMq:Port"]),
                SslEnabled = bool.Parse(builder.Configuration["MqSetting:RabbitMq:SslEnabled"]),
                UserName = builder.Configuration["MqSetting:RabbitMq:UserName"],
            };
            var kafkaSetting = new MessageMiddleware.Kafka.Producers.ProducerOptions
            {
                BootstrapServers = builder.Configuration["MqSetting:Kafka:BootstrapServers"],
                SaslUsername = builder.Configuration["MqSetting:Kafka:SaslUserName"],
                SaslPassword = builder.Configuration["MqSetting:Kafka:SaslPassWord"],
                Key = builder.Configuration["MqSetting:Kafka:Key"]
            };
            var mqConfig = new MQConfig
            {
                ConsumerLog = bool.Parse(builder.Configuration["MqSetting:ConsumerLog"]),
                PublishLog = bool.Parse(builder.Configuration["MqSetting:PublishLog"]),
                Rabbit = rabbitMqSetting,
                Use = int.Parse(builder.Configuration["MqSetting:Use"]),
                Kafka = kafkaSetting
            };
            builder.Services.AddSingleton<MQConfig>(sp => mqConfig);
            builder.Services.AddMQ(mqConfig);
            #endregion
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerDocument();
            
            var app = builder.Build();
            DatabaseStartup.CreateTable(app.Services);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();  
        }
    }
}