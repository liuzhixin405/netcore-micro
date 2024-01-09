using Login.Client.GrpcClient;
using MicroService.Shared.GrpcPool;
using MicroService.Shared;

namespace Login.Client
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
            builder.Services.AddTransient<IGrpcClientFactory<IAccountService>, LoginClientFactory>();
            builder.Services.AddTransient(sp => new GrpcClientPool<IAccountService>(sp.GetService<IGrpcClientFactory<IAccountService>>(), builder.Configuration, builder.Configuration["Grpc:Service:JwtAuthApp.ServiceAddress"]));
            builder.Services.AddSwaggerGen();
            builder.Host.UseOrleansClient(c => {

                c.UseLocalhostClustering(new int[] { 30001, 30002 }); //只有一个生效，而且每次请求各一次才会成功                                                          
            });
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