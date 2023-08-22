using ConsumerClient;
using Microsoft.Extensions.Configuration;
using MessageMiddleware.Factory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region rabbitmqsetting
var rabbitMqSetting = new MessageMiddleware.RabbitMQ.RabbitMQSetting
{
    ConnectionString = builder.Configuration["MqSetting:RabbitMq:ConnectionString"].Split(';'),
    Password = builder.Configuration["MqSetting:RabbitMq:PassWord"],
    Port = int.Parse(builder.Configuration["MqSetting:RabbitMq:Port"]),
    SslEnabled = bool.Parse(builder.Configuration["MqSetting:RabbitMq:SslEnabled"]),
    UserName = builder.Configuration["MqSetting:RabbitMq:UserName"],
};
var kafkaSetting = new MessageMiddleware.Kafka.Producers.ProducerOptions
{
    BootstrapServers = builder.Configuration["MqSetting:Kafka:BootstrapServers"],
    SaslUsername = builder.Configuration["MqSetting:Kafka:SaslUserName"],
    SaslPassword = builder.Configuration["MqSetting:Kafka:SaslPassWord"],
    Key = builder.Configuration["MqSetting:Kafka:Key"]
};
var mqConfig = new MessageMiddleware.Factory.MQConfig
{
    ConsumerLog = bool.Parse(builder.Configuration["MqSetting:ConsumerLog"]),
    PublishLog = bool.Parse(builder.Configuration["MqSetting:PublishLog"]),
    Rabbit = rabbitMqSetting,
    Use = int.Parse(builder.Configuration["MqSetting:Use"]),
    Kafka = kafkaSetting
};
builder.Services.AddSingleton<MessageMiddleware.Factory.MQConfig>(sp => mqConfig);
#endregion
builder.Services.AddMQ(mqConfig);

builder.Services.AddHostedService<OrderConsumer>();
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
