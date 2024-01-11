using System.Threading.Channels;
using Common.DistributedId;
using MagicOnion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Paying.WebApi.BackServices;
using Paying.WebApi.Database;
using Paying.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PaymentContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionString"]);
}, ServiceLifetime.Scoped);
#region 雪花id 分布式
builder.Services.AddDistributedId(new DistributedIdOptions
{
    Distributed = true
});
#endregion
builder.Services.AddTransient<IPayingService ,PayingService>();
builder.Services.AddHostedService<PayTimeoutService>();

var app = builder.Build();
CreateTable(app.Services);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


static void CreateTable(IServiceProvider service)
{
    using var scope = service.CreateScope();
    var context = scope.ServiceProvider.GetService<PaymentContext>();
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