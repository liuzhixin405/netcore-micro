using Microsoft.Extensions.DependencyInjection.Extensions;
using Catalogs.Infrastructure;
using Catalogs.Infrastructure.Database;
using Cache.Options;
using DistributedId;
using Cache;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDB(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<CatalogContextSeed>();
// 添加CORS服务
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.AllowAnyOrigin() // 允许特定的来源
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
#region 雪花id 分布式
builder.Services.AddCache(new CacheOptions
{
    CacheType = CacheTypes.Redis,
    RedisConnectionString = builder.Configuration["DistributedRedis:ConnectionString"] ?? throw new Exception("$未能获取distributedredis连接字符串")
}).AddDistributedId(new DistributedIdOptions
{
    Distributed = true
});
#endregion

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
