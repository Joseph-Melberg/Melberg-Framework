using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.RabbitConsumerWithMetric.Service;
using MelbergFramework.Application;
using MelbergFramework.Infrastructure.Rabbit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Demo.RabbitConsumerWithMetric;
public class Startup : IAppStartup
{
    public  void ConfigureServices(IServiceCollection services)
    {
        RabbitModule.RegisterConsumerWithMetrics<DemoRabbitConsumerWithMetric>(services);
        Register.RegisterServices(services);
    }


}
