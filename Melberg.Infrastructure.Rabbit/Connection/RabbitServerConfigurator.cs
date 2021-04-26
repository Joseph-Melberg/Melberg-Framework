using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melberg.Core.Extensions;
using Melberg.Core.Rabbit.Configurations;
using Melberg.Core.Rabbit.Configurations.Data;
using Melberg.Infrastructure.Rabbit.Extensions;

namespace Melberg.Infrastructure.Rabbit.Connection
{
    public class RabbitServerConfigurator : IServerConfigurator
	{
		private readonly IRabbitConfigurationProvider _amqpRabbitConfigurationProvider;

		public RabbitServerConfigurator(IRabbitConfigurationProvider amqpRabbitConfigurationProvider)
		{
			_amqpRabbitConfigurationProvider = amqpRabbitConfigurationProvider;
		}

		public void InitializeConfiguredObjects(IConnection connection, string connectionName)
		{
			var amqpObject = _amqpRabbitConfigurationProvider.GetAmqpObjectsConfiguration();

			if (amqpObject != null)
			{
				CreateExchanges(connection, connectionName, amqpObject.ExchangeList);
				CreateQueues(connection, connectionName, amqpObject.QueueList);
				CreateBindings(connection, connectionName, amqpObject.BindingList);
			}
		}

		private static void CreateBindings(IConnection connection, string connectionName, IEnumerable<BindingConfigData> list)
		{
			Parallel.ForEach(list.Where(x => x.Connection == connectionName), binding =>
			{
				using (var channel = connection.CreateChannel())
				{
					channel.QueueBind(binding.Queue, binding.Exchange, binding.SubscriptionKey);
				}
			});
		}


		private static void CreateQueues(IConnection connection, string connectionName, IEnumerable<QueueConfigData> list)
		{
            Parallel.ForEach(list.Where(x => x.Connection == connectionName), queue =>
			{
				var args = queue.GetQueueArgs();
                
				using (var channel = connection.CreateChannel())
				{
					channel.QueueDeclare(queue.Name, queue.Durable, queue.Exclusive, queue.AutoDelete, args);
				}
			});
		}


		private static void CreateExchanges(IConnection connection, string connectionName, IEnumerable<ExchangeConfigData> list)
		{
            Parallel.ForEach(list.Where(x => x.Connection == connectionName), exchange =>
			{
				using (var channel = connection.CreateChannel())
				{
					channel.ExchangeDeclare(exchange.Name, exchange.Type.GetDescription().ToLower(), exchange.Durable,
						exchange.AutoDelete, null);
				}
			});
		}
	}
}