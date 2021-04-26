using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Melberg.Infrastructure.Rabbit.Consumers;
using Melberg.Infrastructure.Rabbit.Models;

namespace Melberg.Infrastructure.Rabbit.Connection
{
    public interface IChannel : IDisposable
	{
		void Close(ushort replyCode, string replyText);

        Task ConsumeAsync(
            string queue, 
            ushort prefetch, 
            IMessageConsumer handler, 
            CancellationToken cancellationToken, 
            int checkCancellationInterval, 
            bool noAck);

        void BasicAck(ulong deliveryTag, bool multiple);

		void BasicNack(ulong deliveryTag, bool multiple, bool requeue);

		void BasicReject(ulong deliveryTag, bool requeue);

		void BasicPublish(string exchange, Message message);

        ulong BasicGet(string queue, bool noAck);

        void QueueDeclare(string name, bool durable, bool exclusive, bool autoDelete,
            IDictionary<string, object> arguments);

        void ExchangeDeclare(string exchange, string type, bool durable, bool autoDelete,
             IDictionary<string, object> arguments);

        void QueueBind(string queue, string exchange, string routingKey);

        bool IsClosed { get; }

        IConnection Connection { get; }
    }
}