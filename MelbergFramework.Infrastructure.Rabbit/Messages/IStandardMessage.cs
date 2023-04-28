using System.Collections.Generic;

namespace MelbergFramework.Infrastructure.Rabbit.Messages;

public interface IStandardMessage
{
    IDictionary<string, object> GetHeaders();
    string GetRoutingKey();
}