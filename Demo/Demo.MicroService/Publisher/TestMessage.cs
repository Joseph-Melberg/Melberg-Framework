using MelbergFramework.Infrastructure.Rabbit.Messages;

namespace Demo.Microservice.Publisher;

public class TestMessage : StandardMessage
{
    public override string GetRoutingKey() => "bugos";
}