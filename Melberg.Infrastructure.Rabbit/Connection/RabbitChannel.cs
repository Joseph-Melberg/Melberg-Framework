using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Melberg.Infrastructure.Rabbit.Consumers;
using Melberg.Infrastructure.Rabbit.Models;
using RabbitMQ.Client;

namespace Melberg.Infrastructure.Rabbit.Connection
{
    public class RabbitChannel : IChannel
    {
        private readonly IModel _rabbitChannel;
        private bool _disposed;

        public RabbitChannel(IModel rabbitChannel, IConnection rabbitConnection)
        {
            _rabbitChannel = rabbitChannel;
            Connection = rabbitConnection;
        }

        public void Close(ushort replyCode, string replyText)
        {
            _rabbitChannel.Close(replyCode, replyText);
        }

        public virtual Task ConsumeAsync(
            string queue,
            ushort prefetch,
            IMessageConsumer handler,
            CancellationToken cancellationToken,
            int checkCancellationInterval,
            bool noAck)
        {
            return Task.Factory.StartNew(async () =>
            {
                _rabbitChannel.BasicQos(0, prefetch, false);

                var consumer = new Consumers.QueueingBasicConsumer(_rabbitChannel);

                foreach (var singleQueueName in queue.Split(','))
                {
                    _rabbitChannel.BasicConsume(singleQueueName.Trim(), noAck, consumer);
                }

                while (true)
                {
                    if (cancellationToken.IsCancellationRequested) return;

                    if (consumer.Queue.Dequeue(checkCancellationInterval, out var deliveryArgs))
                    {
                        var message = new Message()
                        {
                            Body = deliveryArgs.Body,
                            RoutingKey = deliveryArgs.RoutingKey,
                            Headers = deliveryArgs.BasicProperties.Headers,
                            ContentEncoding = deliveryArgs.BasicProperties.ContentEncoding,
                            ContentType = deliveryArgs.BasicProperties.ContentType,
                            DeliveryMode = deliveryArgs.BasicProperties.DeliveryMode,
                            DeliveryHeader = new DeliveryHeader
                            {
                                Redelivered = deliveryArgs.Redelivered,
                                DeliveryTag = deliveryArgs.DeliveryTag
                            }
                        };

                        await handler.ConsumeMessageAsync(message, this, cancellationToken);
                    }
                }
            }, cancellationToken).Unwrap();
        }


        public void BasicAck(ulong deliveryTag, bool multiple)
        {
            _rabbitChannel.BasicAck(deliveryTag, multiple);
        }

        public void BasicNack(ulong deliveryTag, bool multiple, bool requeue)
        {
            _rabbitChannel.BasicNack(deliveryTag, multiple, requeue);
        }

        public void BasicReject(ulong deliveryTag, bool requeue)
        {
            _rabbitChannel.BasicReject(deliveryTag, requeue);
        }

        public ulong BasicGet(string queue, bool noAck)
        {
            var getResult = _rabbitChannel.BasicGet(queue, noAck);
            return getResult.DeliveryTag;
        }

        public void BasicPublish(string exchange, Message message)
        {
            var properties = _rabbitChannel.CreateBasicProperties();

            properties.ContentType = message.ContentType;
            properties.ContentEncoding = message.ContentEncoding;
            properties.Headers = message.Headers;
            properties.DeliveryMode = message.DeliveryMode;

            if (message.ExpirationInMs.HasValue)
            {
                properties.Expiration = message.ExpirationInMs.Value.ToString();
            }

            if (message.Priority.HasValue)
            {
                properties.Priority = message.Priority.Value;
            }

            _rabbitChannel.BasicPublish(exchange, message.RoutingKey, properties, message.Body);
        }

        public void QueueDeclare(string name, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            _rabbitChannel.QueueDeclare(name, durable, exclusive, autoDelete, arguments);
        }

        public void ExchangeDeclare(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            _rabbitChannel.ExchangeDeclare(exchange, type, durable, autoDelete, arguments);
        }

        public void QueueBind(string queue, string exchange, string routingKey)
        {
            _rabbitChannel.QueueBind(queue, exchange, routingKey);
        }

        public bool IsClosed => _rabbitChannel.IsClosed;

        public IConnection Connection { get; }

        public IModel GetChannel() => _rabbitChannel;

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    _rabbitChannel.Close();
                    _rabbitChannel.Dispose();
                }
                catch
                {
                }
            }

            _disposed = true;
        }

        #endregion
    }
}