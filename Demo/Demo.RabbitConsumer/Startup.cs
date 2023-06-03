using System;
using System.Threading;
using System.Threading.Tasks;
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
        services.AddHostedService<Test>();
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