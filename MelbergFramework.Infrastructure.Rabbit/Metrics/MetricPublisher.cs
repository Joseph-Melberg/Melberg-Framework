using System;
using MelbergFramework.Core.Application;
using MelbergFramework.Infrastructure.Rabbit.Publishers;
using Microsoft.Extensions.Options;

namespace MelbergFramework.Infrastructure.Rabbit.Metrics;

public class MetricPublisher : IMetricPublisher
{
    private readonly IStandardPublisher<MetricMessage> _metricPublisher;
    private readonly ApplicationConfiguration _config;

    public MetricPublisher(
        IStandardPublisher<MetricMessage> publisher,
        IOptions<ApplicationConfiguration> config)
    {
        _metricPublisher = publisher;
        _config = config.Value;
    }

    public void SendMetric(long timeInMS, DateTime timeStamp) => 
        _metricPublisher.Send(
            new MetricMessage()
            {
                Application = _config.Name,
                TimeInMS = timeInMS,
                TimeStamp = timeStamp
            });
}