using System;
using System.Threading;
using System.Threading.Tasks;
using MelbergFramework.Core.Health;
using MelbergFramework.Infrastructure.Rabbit.Factory;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MelbergFramework.Infrastructure.Rabbit.Health;

public class RabbitPublisherHealthCheck<TMessage> : HealthCheck
{
    private readonly IConnection _connection;
    public RabbitPublisherHealthCheck(IStandardConnectionFactory factory)
    {
        _connection = factory.GetPublisherChannel(typeof(TMessage).Name);
    }
    public override string Name => "rabbitpublisher_"+typeof(TMessage).Name;

    public override Task<bool> IsOk(CancellationToken token)
    {

        return Task.FromResult(_connection.IsOpen);
    }
}