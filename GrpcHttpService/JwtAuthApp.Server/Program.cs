using System.Net;
using JwtAuthApp.Server.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthApp.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.WebHost.ConfigureKestrel(options =>
            //{
            //    //options.Listen(IPAddress.Any, 7021, endpointOptions =>
            //    //{
            //    //    endpointOptions.Protocols = HttpProtocols.Http2;
            //    //    endpointOptions.UseHttps();
            //    //});
            //});
            builder.Services.AddGrpc();
            builder.Services.AddMagicOnion();

            builder.Services.AddSingleton<JwtTokenService>();
            builder.Services.Configure<JwtTokenServiceOptions>(builder.Configuration.GetSection("JwtAuthApp.Server:JwtTokenService"));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration.GetSection("JwtAuthApp.Server:JwtTokenService:Secret").Value!)),
                        RequireExpirationTime = true,
                        RequireSignedTokens = true,
                        ClockSkew = TimeSpan.FromSeconds(10),

                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                    };
#if DEBUG
                    options.RequireHttpsMetadata = false;
#endif
                });
            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();
            app.MapMagicOnionService();
            app.Run();
        }
    }
}