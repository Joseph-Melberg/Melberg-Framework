using System.Collections.Generic;

namespace Melberg.Infrastructure.Rabbit.Messages;

public interface IStandardMessage
{
    IDictionary<string, object> GetHeaders();
    string GetRoutingKey();
}