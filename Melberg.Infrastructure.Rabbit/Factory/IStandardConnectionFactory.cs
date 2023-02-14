using RabbitMQ.Client;

namespace Melberg.Infrastructure.Rabbit.Factory;

public interface IStandardConnectionFactory
{
    IConnection GetConsumerChannel();
    IConnection GetPublisherChannel(string name);
}