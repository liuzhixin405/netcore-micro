using Microsoft.Extensions.DependencyInjection.Extensions;
using Catalogs.Infrastructure;
using Catalogs.Infrastructure.Database;
using DistributedId;
using Catalogs.WebApi.BackgroudServices;
using System.Threading.Channels;
using System.Text.Json;
using Common.Redis.Extensions;
using Common.Redis.Extensions.Configuration;
using Common.Redis.Extensions.Serializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // 使用小驼峰命名策略
//})
    ;
builder.Services.AddDB(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<CatalogContextSeed>();
builder.Services.AddSingleton(Channel.CreateUnbounded<string>());
// 添加CORS服务
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
#region 雪花id 分布式
builder.Services.AddDistributedId(new DistributedIdOptions
{
    Distributed = true
});
#endregion
//redis
builder.Services.AddSingleton<IRedisCache>(obj =>
{
    var config = builder.Configuration.GetSection("Redis").Get<RedisConfiguration>();
    var serializer = new MsgPackSerializer();
    var connection = new PooledConnectionMultiplexer(config.ConfigurationOptions);
    return new RedisCache(obj.GetService<ILoggerFactory>().CreateLogger<RedisCache>(), connection, config, serializer);
});

builder.Services.AddHostedService<InitProductListToRedisService>();
var app = builder.Build();
ApplicationStartup.CreateTable(app.Services);
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seedService = services.GetService<CatalogContextSeed>();
    seedService.SeedAsync().Wait();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// 启用CORS中间件
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
