using System.Collections.Generic;
using System.Linq;
using Melberg.Core.Rabbit.Configurations.Data;
using RabbitMQ.Client;

namespace Melberg.Infrastructure.Rabbit.Configuration
{
    public static class RabbitConfigurator
    {
        public static void ConfigureExchanges(this IModel Channel, string Connection, IEnumerable<ExchangeConfigData> ExchangeInfo)
        {
            var relevantExchanges = ExchangeInfo.Where(_ => _.Connection == Connection).ToList();

            foreach(var exchange in relevantExchanges)
            {
                Channel.ExchangeDeclare(exchange.Name,exchange.Type.ToExchangeType(),exchange.Durable,exchange.AutoDelete);
            }
        }

        public static void ConfigureQueues(this IModel Channel, string Connection, IEnumerable<QueueConfigData> QueueData)
        {
            var relevantQueues = QueueData.Where(_ => _.Connection == Connection).ToList();

            foreach(var queue in relevantQueues)
            {
                Channel.QueueDeclare(queue.Name,queue.Durable,queue.Exclusive,queue.AutoDelete);
            }
        }

        public static void ConfigureBindings(this IModel Channel, string Connection, IEnumerable<BindingConfigData> BindingData)
        {
            var relevantBindings = BindingData.Where(_ => _.Connection == Connection).ToList();
            foreach(var binding in relevantBindings)
            {
                Channel.QueueBind(binding.Queue,binding.Exchange,binding.SubscriptionKey);
            }
        }
        public static string ToExchangeType(this ExchangeConfigType ConfigType)
        {
            switch(ConfigType)
            {
                case ExchangeConfigType.Direct: return ExchangeType.Direct;
                case ExchangeConfigType.Fanout: return ExchangeType.Fanout;
                case ExchangeConfigType.Topic:  return ExchangeType.Topic;
            }
            throw new System.Exception("An invalid exchange type has been given");
        }
    }
}