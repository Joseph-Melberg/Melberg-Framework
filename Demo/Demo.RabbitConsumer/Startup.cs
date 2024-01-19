using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.RabbitConsumer.Messages;
using Demo.RabbitConsumer.Service;
using MelbergFramework.Application;
using MelbergFramework.Infrastructure.Rabbit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.RabbitConsumer;

public class Startup : IAppStartup
{
    public  void ConfigureServices(IServiceCollection services)
    {
        RabbitModule.RegisterConsumer<DemoRabbitConsumer>(services);

        RabbitModule.RegisterMicroConsumer<DemoRabbitAltConsumer, TestMessage>(services, false);

        Register.RegisterServices(services);
    }


}

public class Test : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Allow");
        return Task.CompletedTask;
    
    }
}