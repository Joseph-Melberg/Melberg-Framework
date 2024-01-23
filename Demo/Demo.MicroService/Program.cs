using System.Collections;
using System.Diagnostics;
using Demo.Microservice;
using Demo.Microservice.Publisher;
using Demo.Microservice.Redis;
using MelbergFramework.Application;
using MelbergFramework.Infrastructure.Rabbit;
using MelbergFramework.Infrastructure.Rabbit.Messages;
using MelbergFramework.Infrastructure.Redis;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterRequired();

builder.Services.AddTransient<IPublisherTest, PublisherTest>();
RabbitModule.RegisterPublisher<TestMessage>(builder.Services);
RedisModule.LoadRedisRepository<IDemoRedisRepository, DemoRedisRepository,DemoRedisContext>(builder.Services);

RabbitModule.RegisterMicroConsumer<TestPillar, TickMessage>(builder.Services, !builder.Environment.IsDevelopment());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsDevelopment())
{
    app.Configuration["Rabbit:ClientDeclarations:Connections:0:Password"] = app.Configuration["rabbit_pass"];
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var sto = new Stopwatch();

var repo = app.Services.GetRequiredService<IDemoRedisRepository>();

repo.Test();

while (true)
{
    sto.Restart();
    using (var scope = app.Services.CreateScope())
    {
        var pub = scope.ServiceProvider.GetService<IPublisherTest>();
        pub.Send();
    }
    Console.WriteLine(sto.ElapsedMilliseconds);

}
app.Run();
