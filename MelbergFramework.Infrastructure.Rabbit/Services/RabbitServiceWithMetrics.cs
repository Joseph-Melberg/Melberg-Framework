using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MelbergFramework.Core.Rabbit;
using MelbergFramework.Core.Rabbit.Configurations;
using MelbergFramework.Infrastructure.Rabbit.Consumers;
using MelbergFramework.Infrastructure.Rabbit.Factory;
using MelbergFramework.Infrastructure.Rabbit.Messages;
using MelbergFramework.Infrastructure.Rabbit.Metrics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MelbergFramework.Infrastructure.Rabbit;

public class RabbitServiceWithMetrics : RabbitService
{
    private readonly IMetricPublisher _metricPublisher;
    public RabbitServiceWithMetrics(
        IMetricPublisher metricPublisher,
        IStandardConsumer consumer,
        IRabbitConfigurationProvider configurationProvider,
        IStandardConnectionFactory connectionFactory,
        IOptions<RabbitConsumerConfiguration> consumerConfiguration,
        ILogger logger) : base(
            consumer,
            configurationProvider,
            connectionFactory,
            consumerConfiguration,
            logger)
    {
        _metricPublisher = metricPublisher;
    }

    public override async Task ConsumeMessageAsync(Message message, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var timer = new Stopwatch();
        timer.Start();
        await base.ConsumeMessageAsync(message, cancellationToken);
        timer.Stop();
    
        _metricPublisher.SendMetric(timer.ElapsedMilliseconds, now);
    }
}
