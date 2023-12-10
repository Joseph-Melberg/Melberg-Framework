using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.RabbitConsumerWithMetric.Messages;
using MelbergFramework.Core.Application;
using MelbergFramework.Infrastructure.Rabbit;
using MelbergFramework.Infrastructure.Rabbit.Consumers;
using MelbergFramework.Infrastructure.Rabbit.Messages;
using MelbergFramework.Infrastructure.Rabbit.Translator;
using Microsoft.Extensions.Options;
namespace Demo.RabbitConsumerWithMetric.Service;

public class DemoRabbitConsumerWithMetric : IStandardConsumer
{

    private readonly IJsonToObjectTranslator<TestMessage> _translator;
    private readonly IThing _thing;
    public DemoRabbitConsumerWithMetric(
        IJsonToObjectTranslator<TestMessage> translator, 
        IOptions<ApplicationConfiguration> config,
        IThing thing
    )
    {
        _thing = thing;
        _translator = translator;
    }

    public async Task ConsumeMessageAsync(Message message, CancellationToken ct)
    {
        Console.WriteLine(_thing.Value);
        var result = _translator.Translate(message);

        await Task.Delay(10);
    }
}