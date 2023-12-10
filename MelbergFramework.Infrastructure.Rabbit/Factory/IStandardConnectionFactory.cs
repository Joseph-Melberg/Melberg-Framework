using RabbitMQ.Client;

namespace MelbergFramework.Infrastructure.Rabbit.Factory;

public interface IStandardConnectionFactory
{
    IModel GetConsumerModel(string name = "IncomingMessages");
    IConnection GetPublisherChannel(string name);
}