using System;
using MelbergFramework.Infrastructure.Rabbit.Messages;

namespace MelbergFramework.Infrastructure.Rabbit.Metrics;

public class MetricMessage : StandardMessage
{
    public string Application;
    public DateTime TimeStamp;
    public long TimeInMS;
    public override string GetRoutingKey() => "metric";
}