using System;

namespace MelbergFramework.Infrastructure.Rabbit.Metrics;   

public interface IMetricPublisher
{
    void SendMetric(long timeInMS, DateTime timeStamp);
}