using Melberg.Infrastructure.Rabbit.Messages;

namespace Demo.RabbitPublisher;

public class TestMessage : IStandardMessage
{
    public string Body => "Body";


    public string GetRoutingKey() => "test";
}