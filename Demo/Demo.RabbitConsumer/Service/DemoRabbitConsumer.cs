using System;
using System.Threading.Tasks;
using Demo.RabbitConsumer.Messages;
using Melberg.Infrastructure.Rabbit.Consumers;
using Melberg.Infrastructure.Rabbit.Messages;
using Melberg.Infrastructure.Rabbit.Translator;

namespace Demo.RabbitConsumer.Service
{
    public class DemoRabbitConsumer : IStandardConsumer
    {
        private readonly IJsonToObjectTranslator<TestMessage> _translator;
        public DemoRabbitConsumer(
            IJsonToObjectTranslator<TestMessage> translator
        )
        {
            _translator = translator;
        }
        public Task ConsumeMessageAsync(Message message)
        {
            Console.WriteLine(message);
            var result = _translator.Translate(message);


            return Task.CompletedTask;
        }
    }
}