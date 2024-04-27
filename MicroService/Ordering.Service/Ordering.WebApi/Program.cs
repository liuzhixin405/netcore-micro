using NSwag;
using Common.DistributedId;
using Ordering.WebApi.Filters;
using MagicOnion;
using Ordering.Infrastructure.Extensions;
using Ordering.WebApi.OutBoxMessageServices;
using Common.MessageMiddleware.Extensions;
using Ordering.Infrastructure;
using Common.Redis.Extensions.Configuration;
using Common.Redis.Extensions.Serializer;
using Common.Redis.Extensions;
using Ordering.IGrain;
using Orleans.Configuration;
using Orleans.Serialization;
using Ordering.WebApi.Services.Orders;
using Common.Util.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CommonExceptionFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddGrpc();
builder.Services.AddMagicOnion();
#region Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.AllowAnyOrigin() // 允许所有来源
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
#endregion

builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "后台管理系统";
    settings.AllowReferencesWithProperties = true;
});
#region 雪花id 分布式
builder.Services.AddDistributedId(new DistributedIdOptions
{
    Distributed = true
});
#endregion
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddMq(builder.Configuration);
//redis
builder.Services.AddSingleton<IRedisCache>(obj =>
{
    var config = builder.Configuration.GetSection("Redis").Get<RedisConfiguration>()??throw new SystemErrorException("redis配置有误,请检查后重试");
    var serializer = new MsgPackSerializer();
    var connection = new PooledConnectionMultiplexer(config.ConfigurationOptions);
    return new RedisCache(obj.GetService<ILoggerFactory>().CreateLogger<RedisCache>()?? throw new SystemErrorException("rediscache创建失败,请检查后重试"), connection, config, serializer);
});
builder.Services.AddSerializer(sb=>
{
    sb.AddJsonSerializer(
       isSupported: type => type.Namespace.StartsWith("Ordering"));
}); // 该配置和序列化对象需要特殊配置,跟grpc一样。
builder.Host.UseOrleans(clientBuilder =>
{
    clientBuilder
        .UseLocalhostClustering()
        .Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "order";
            options.ServiceId = "ordering.webapi";
        });
});
builder.Services.AddHostedService<CreateOrderbService>();
builder.Services.AddTransient<IOrderService,OrderService>();
var app = builder.Build();
ApplicationStartup.CreateTable(app.Services);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}
app.UseCors("AllowSpecificOrigin");

app.UseRouting();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapMagicOnionService();
app.Run();