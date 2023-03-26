using RabbitMQ.Client;

namespace Melberg.Infrastructure.Rabbit.Factory;

public interface IStandardConnectionFactory
{
    IModel GetConsumerModel();
    IConnection GetPublisherChannel(string name);
}