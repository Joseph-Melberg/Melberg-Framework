using System.Threading;
using System.Threading.Tasks;
using MelbergFramework.Core.Rabbit.Configurations;
using MelbergFramework.Infrastructure.Rabbit;
using MelbergFramework.Infrastructure.Rabbit.Consumers;
using MelbergFramework.Infrastructure.Rabbit.Factory;
using MelbergFramework.Infrastructure.Rabbit.Messages;
using Microsoft.Extensions.Logging;

namespace Demo.RabbitConsumer.Service;

public class DemoRabbitAltConsumer : IStandardConsumer
{
    public DemoRabbitAltConsumer()
    {
    }

    public Task ConsumeMessageAsync(Message message, CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }
}