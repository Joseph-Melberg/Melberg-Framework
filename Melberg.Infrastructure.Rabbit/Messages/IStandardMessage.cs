namespace Melberg.Infrastructure.Rabbit.Messages;

public interface IStandardMessage
{
    string Body {get;}
    string GetRoutingKey();
}