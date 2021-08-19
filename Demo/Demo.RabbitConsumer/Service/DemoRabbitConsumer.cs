using System;
using System.Threading.Tasks;
using Melberg.Infrastructure.Rabbit.Consumers;

namespace Demo.RabbitConsumer.Service
{
    public class DemoRabbitConsumer : IStandardConsumer
    {
        public Task ConsumeMessageAsync(string message)
        {
            Console.WriteLine(message);

            return Task.CompletedTask;
        }
    }
}