namespace Melberg.Infrastructure.Rabbit.Consumers
{
    public abstract class RabbitConsumer
    {
        public RabbitConsumer()
        {

        }

        public abstract void HandleMessage(Message message)
        {

        }
    }
}