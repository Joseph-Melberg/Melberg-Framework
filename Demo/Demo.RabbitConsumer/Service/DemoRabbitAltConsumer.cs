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
    private ILogger _logger;
    public DemoRabbitAltConsumer(ILogger logger)
    {
        _logger = logger;
        _logger.LogInformation("A");
    }

    public Task ConsumeMessageAsync(Message message, CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }
}