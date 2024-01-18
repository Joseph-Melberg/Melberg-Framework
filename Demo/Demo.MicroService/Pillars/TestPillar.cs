using MelbergFramework.Infrastructure.Rabbit.Consumers;
using MelbergFramework.Infrastructure.Rabbit.Messages;

namespace Demo.Microservice;

public class TestPillar : IStandardConsumer
{
    public Task ConsumeMessageAsync(Message message, CancellationToken ct)
    {
        return Task.Delay(1);
    }
}