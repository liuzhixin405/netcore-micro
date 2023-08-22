using Common.Util.Es.Foundation;
using MessageMiddleware.Factory;
using MessageMiddleware.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Context;
using project.Elasticsearchs.Product.Search;
using project.Filters;
using project.Repositories;
using project.SeedWork;
using project.Services;
using project.Utility.Helper;
using RepositoryComponent.DbFactories;
using StackExchange.Redis.Extensions.Core.Implementations;

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

            builder.Services.AddScoped<Func<ReadProductDbContext>>(provider => () => provider.GetService<ReadProductDbContext>() ?? throw new ArgumentNullException("ReadProductDbContext is not inject to program"));
            builder.Services.AddScoped<Func<WriteProductDbContext>>(provider => () => provider.GetService<WriteProductDbContext>() ?? throw new ArgumentNullException("WriteProductDbContext is not inject to program"));

            builder.Services.AddScoped<DbFactory<WriteProductDbContext>>();
            builder.Services.AddScoped<DbFactory<ReadProductDbContext>>();

            builder.Services.AddTransient<IReadProductRepository, ProductReadRepository>();
            builder.Services.AddTransient<IWriteProductRepository, ProductWriteRepository>();
            builder.Services.AddTransient<IProductService, ProductService>();

            builder.Services.AddTransient<ICustomerService, CustomerService>();

            #region redis
            var message = string.Empty;
            Task.WaitAny(new Task[]{ Task.Run(() => {
               CacheHelper.Init(builder.Configuration); //有可能初始化失败
             return Task.CompletedTask;
              }), Task.Run(async () => {
             await Task.Delay(5000);
                message =$"{nameof(CacheHelper)} 初始化失败,请重试";
             }) });
            if (!string.IsNullOrEmpty(message)) throw new Exception(message);
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

            #region essearch
            var section = builder.Configuration.GetSection("EsConfig");
            builder.Services.AddSingleton(new EsConfig(section.Get<EsOption>()));
            builder.Services.AddTransient<EsProductContext>();
            #endregion
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerDocument();
            //builder.Services.AddProblemDetails();
            var app = builder.Build();
            DatabaseStartup.CreateTable(app.Services);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}