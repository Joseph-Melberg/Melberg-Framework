using MelbergFramework.Infrastructure.Rabbit.Messages;
namespace Demo.RabbitConsumerWithMetric.Messages;

public class TestMessage : StandardMessage
{
    public string RoutingKey {get; set;} 
    public string Value {get; set;}

    public override string GetRoutingKey() => RoutingKey;
}