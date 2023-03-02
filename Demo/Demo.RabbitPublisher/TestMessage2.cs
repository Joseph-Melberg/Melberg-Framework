using Melberg.Infrastructure.Rabbit.Messages;

namespace Demo.RabbitPublisher;

public class TestMessage2 : StandardMessage
{
    public string Value {get; set;}
    public override string GetRoutingKey() => "test";
}