using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.RabbitConsumer.Messages;
using MelbergFramework.Core.Application;
using MelbergFramework.Infrastructure.Rabbit.Consumers;
using MelbergFramework.Infrastructure.Rabbit.Messages;
using MelbergFramework.Infrastructure.Rabbit.Translator;
using Microsoft.Extensions.Options;

namespace Demo.RabbitConsumer.Service
{
    public class DemoRabbitConsumer : IStandardConsumer
    {
        private readonly IJsonToObjectTranslator<TestMessage> _translator;
        public DemoRabbitConsumer(
            IJsonToObjectTranslator<TestMessage> translator, 
            IOptions<ApplicationConfiguration> config
        )
        {
            _translator = translator;
        }
        public Task ConsumeMessageAsync(Message message, CancellationToken ct)
        {
            Console.WriteLine(message);
            var result = _translator.Translate(message);


            return Task.CompletedTask;
        }
    }
}