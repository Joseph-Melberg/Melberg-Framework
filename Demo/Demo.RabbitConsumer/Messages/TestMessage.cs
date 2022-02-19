using Melberg.Infrastructure.Rabbit.Messages;

namespace Demo.RabbitConsumer.Messages;

public class TestMessage : StandardMessage 
{
    
    public string RoutingKey {get; set;} 
    public string Value {get; set;}

    public override string GetRoutingKey() => RoutingKey;
}