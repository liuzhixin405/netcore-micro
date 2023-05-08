
using EfCoreProject.Context;
using EfCoreProject.DbFactories;
using EfCoreProject.Models;
using EfCoreProject.Repositories;
using EfCoreProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EfCoreProject
{
    public class Program       //chatgpt只能写基础的逻辑代码
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            ///sqlserver   
            //builder.Services.AddDbContext<ReadProductDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:SqlServer:ReadConnection"]),ServiceLifetime.Scoped);
            //builder.Services.AddDbContext<WriteProductDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:SqlServer:WriteConnection"]), ServiceLifetime.Scoped);
            ///mysql
            builder.Services.AddDbContext<ReadProductDbContext>(options => options.UseMySQL(builder.Configuration["ConnectionStrings:MySql:ReadConnection"]), ServiceLifetime.Scoped);
            builder.Services.AddDbContext<WriteProductDbContext>(options => options.UseMySQL(builder.Configuration["ConnectionStrings:MySql:WriteConnection"]), ServiceLifetime.Scoped);


            builder.Services.AddScoped<Func<ReadProductDbContext>>(provider => () =>provider.GetService<ReadProductDbContext>()??throw new ArgumentNullException("ReadProductDbContext is not inject to program"));
            builder.Services.AddScoped<Func<WriteProductDbContext>>(provider => () => provider.GetService<WriteProductDbContext>() ?? throw new ArgumentNullException("WriteProductDbContext is not inject to program"));

            builder.Services.AddScoped<DbFactory<WriteProductDbContext>>();
            builder.Services.AddScoped<DbFactory<ReadProductDbContext>>();

            builder.Services.AddTransient<IReadProductRepository,ProductReadRepository>();
            builder.Services.AddTransient<IWriteProductRepository, ProductWriteRepository>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
        }
    }
}