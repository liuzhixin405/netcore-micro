
using chatgptwriteproject.Context;
using chatgptwriteproject.DbFactories;
using chatgptwriteproject.Models;
using chatgptwriteproject.Repositories;
using chatgptwriteproject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace chatgptwriteproject
{
    public class Program       //chatgpt只能写基础的逻辑代码
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
                 
            builder.Services.AddDbContextFactory<ReadProductDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:ReadConnection"]),ServiceLifetime.Scoped);
            builder.Services.AddDbContextFactory<WriteProductDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:WriteConnection"]), ServiceLifetime.Scoped);
            
            builder.Services.AddScoped<Func<Tuple<ReadProductDbContext, WriteProductDbContext>>>(provider => () =>Tuple.Create(provider.GetService<ReadProductDbContext>()??throw new ArgumentNullException("ReadProductDbContext is not inject to program"),provider.GetService<WriteProductDbContext>() ?? throw new ArgumentNullException("WriteProductDbContext is not inject to program")));
          
            builder.Services.AddScoped<DbFactory<ReadProductDbContext,WriteProductDbContext>>();
            builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
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