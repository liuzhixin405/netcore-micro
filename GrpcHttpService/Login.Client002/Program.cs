using Login.Client002.GrpcClient;
using MicroService.Shared;
using MicroService.Shared.GrpcPool;
using Microsoft.Extensions.Configuration;

namespace Login.Client002
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<IGrpcClientFactory<IAccountService>,LoginClientFactory>();
            builder.Services.AddTransient(sp=>new GrpcClientPool<IAccountService>(sp.GetService<IGrpcClientFactory<IAccountService>>(), builder.Configuration, builder.Configuration["Grpc:Service:JwtAuthApp.ServiceAddress"]));
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