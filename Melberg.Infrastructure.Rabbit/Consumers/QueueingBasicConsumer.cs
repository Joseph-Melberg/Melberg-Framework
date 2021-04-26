using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Melberg.Infrastructure.Rabbit.Consumers
{
    public class QueueingBasicConsumer : DefaultBasicConsumer
    {
        public QueueingBasicConsumer() : this(null)
        {
        }

        public QueueingBasicConsumer(IModel model) : this(model, new SharedQueue<BasicDeliverEventArgs>())
        {
        }

        public QueueingBasicConsumer(IModel model, SharedQueue<BasicDeliverEventArgs> queue) : base(model)
        {
            Queue = queue;
        }

        public SharedQueue<BasicDeliverEventArgs> Queue { get; protected set; }

        /// <summary>
        /// Overrides <see cref="DefaultBasicConsumer"/>'s  <see cref="HandleBasicDeliver"/> implementation,
        ///  building a <see cref="BasicDeliverEventArgs"/> instance and placing it in the Queue.
        /// </summary>
        public void HandleBasicDeliver(string consumerTag,
            ulong deliveryTag,
            bool redelivered,
            string exchange,
            string routingKey,
            IBasicProperties properties,
            byte[] body)
        {
            var eventArgs = new BasicDeliverEventArgs
            {
                ConsumerTag = consumerTag,
                DeliveryTag = deliveryTag,
                Redelivered = redelivered,
                Exchange = exchange,
                RoutingKey = routingKey,
                BasicProperties = properties,
                Body = body
            };
            Queue.Enqueue(eventArgs);
        }

        /// <summary>
        /// Overrides <see cref="DefaultBasicConsumer"/>'s OnCancel implementation,
        ///  extending it to call the Close() method of the <see cref="SharedQueue"/>.
        /// </summary>
        public void OnCancel()
        {
            base.OnCancel();
            Queue.Close();
        }
    }
}