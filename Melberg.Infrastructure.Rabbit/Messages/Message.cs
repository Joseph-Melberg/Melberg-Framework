using System;
using System.Collections.Generic;
using Melberg.Infrastructure.Rabbit.Extensions;

namespace Melberg.Infrastructure.Rabbit.Messages;

public class Message
{
    public IDictionary<string, object> Headers { get; set; }

    public byte[] Body { get; set; }
    public string RoutingKey { get; set; }
    public DateTime Timestamp => this.GetTimestamp(); 

}