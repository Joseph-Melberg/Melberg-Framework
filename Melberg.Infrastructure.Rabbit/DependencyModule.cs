using Melberg.Core.Rabbit.Configurations;
using Melberg.Infrastructure.Rabbit.Configuration;
using Melberg.Infrastructure.Rabbit.Consumers;
using Melberg.Infrastructure.Rabbit.Messages;
using Melberg.Infrastructure.Rabbit.Publishers;
using Microsoft.Extensions.DependencyInjection;

namespace Melberg.Infrastructure.Rabbit
{
    public static class RabbitModule
    {
        public static void RegisterConsumer<TConsumer>(IServiceCollection catalog)
        where TConsumer : class, IStandardConsumer
        {
            catalog.AddTransient<IStandardConsumer,TConsumer>();
            catalog.AddSingleton<IRabbitConfigurationProvider,RabbitConfigurationProvider>();
            catalog.AddHostedService<RabbitService>();
        }

        public static void RegisterPublisher<TMessage>(IServiceCollection catalog)
            where TMessage : IStandardMessage
        {
            catalog.AddTransient<IStandardPublisher<TMessage>,StandardPublisher<TMessage>>();
            catalog.AddSingleton<IRabbitConfigurationProvider,RabbitConfigurationProvider>();
        }
    }
}