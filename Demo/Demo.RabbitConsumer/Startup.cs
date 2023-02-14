using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.RabbitConsumer.Service;
using Melberg.Application;
using Melberg.Application.Health;
using Melberg.Core.Health;
using Melberg.Infrastructure.Rabbit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.RabbitConsumer;

public class Startup : IAppStartup
{
    public  void ConfigureServices(IServiceCollection services)
    {
        RabbitModule.RegisterConsumer<DemoRabbitConsumer>(services);
        services.AddHostedService<Test>();
        ApplicationModule.AddKeepAlive(services);
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