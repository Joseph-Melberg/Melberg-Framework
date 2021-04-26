using Melberg.Core.Rabbit.Configurations;

namespace Melberg.Infrastructure.Rabbit.Configuration
{
    public class ConsumerConfiguration : IConsumerConfiguration
    {
        public string GetReceiverName() => "Incoming Messages";
    }
}