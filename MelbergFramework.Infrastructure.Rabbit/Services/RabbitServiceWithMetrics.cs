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
    private readonly ILogger _logger;
    public RabbitServiceWithMetrics(
        IServiceProvider provider,
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
            provider,
            logger)
    {
        _metricPublisher = metricPublisher;
        _logger = logger;
    }

    public override Task HandleMessage(object ch, object ea)
    {
        var now = DateTime.UtcNow;
        var timer = new Stopwatch();
        timer.Start();
        await base.ConsumeMessageAsync(message, cancellationToken);
        timer.Stop();
    
        try
        {
            _metricPublisher.SendMetric(timer.ElapsedMilliseconds, now);
        }
        catch (System.Exception ex)
        {
            _logger.LogError($"Couldn't send metric message, {ex}");
        }
    }
}