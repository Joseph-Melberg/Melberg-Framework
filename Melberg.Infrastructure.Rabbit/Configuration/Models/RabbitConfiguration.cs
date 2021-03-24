using Melberg.Infrastructure.Rabbit.Configuration.Models;

namespace Melberg.Infrastructure.Rabbit.Models
{
    public class RabbitConfiguration
    {
        public AmqpConnections AmqpConnections {get; set;}

        public AmqpServerObjects AmqpServerObjects {get; set;}

        public const string ConfigurationName = "RabbitConfiguration";
    }    
}