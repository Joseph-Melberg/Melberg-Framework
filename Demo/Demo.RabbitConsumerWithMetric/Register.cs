using Demo.RabbitConsumerWithMetric.Messages;
using Demo.RabbitConsumerWithMetric.Service;
using MelbergFramework.Infrastructure.Rabbit;
using MelbergFramework.Infrastructure.Rabbit.Translator;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.RabbitConsumerWithMetric;

public class Register
{
    public static IServiceCollection RegisterServices(IServiceCollection services)
    {
        RabbitModule.RegisterConsumerWithMetrics<DemoRabbitConsumerWithMetric>(services);
        services.AddTransient<IJsonToObjectTranslator<TestMessage>,JsonToObjectTranslator<TestMessage>>();
        return services;
    }
}