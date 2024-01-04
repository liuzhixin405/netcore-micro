using Cache.Options;
using Cache;
using Common.Util.Jwt;
using Customers.Center.Service;
using Customers.Domain.Customers;
using Customers.Domain.Seedwork;
using Customers.Infrastructure;
using Customers.Infrastructure.Domain;
using Customers.Infrastructure.Domain.Customers;
using DistributedId;
using NetCore.AutoRegisterDi;
using NSwag;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "后台管理系统";
    settings.AllowReferencesWithProperties = true;
    settings.AddSecurity("身份认证Token", Enumerable.Empty<string>(), new OpenApiSecurityScheme()
    {
        Scheme = "bearer",
        Description = "Authorization:Bearer {your JWT token}<br/><b>授权地址:/Token/GetToken</b>",
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Type = OpenApiSecuritySchemeType.Http
    });
});
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddDB(builder.Configuration);

builder.Services.AddGrpc();
builder.Services.AddMagicOnion();
//builder.Services.RegisterAssemblyPublicNonGenericClasses();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
var app = builder.Build();
ApplicationStartup.CreateTable(app.Services);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}
// 启用CORS中间件
app.UseCors("AllowSpecificOrigin");
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers().RequireAuthorization();
});
//app.MapControllers();
app.MapMagicOnionService();
app.Run();
