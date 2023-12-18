using System;

namespace MelbergFramework.Infrastructure.Rabbit.Metrics;   

public interface IMetricPublisher
{
    void SendMetric(string metric, long timeInMS, DateTime timeStamp);
}