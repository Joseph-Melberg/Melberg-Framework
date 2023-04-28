using RabbitMQ.Client;

namespace MelbergFramework.Infrastructure.Rabbit.Factory;

public interface IStandardConnectionFactory
{
    IModel GetConsumerModel();
    IConnection GetPublisherChannel(string name);
}