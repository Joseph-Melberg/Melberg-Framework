using System;
using System.Threading;
using System.Threading.Tasks;
using Melberg.Core.Health;
using Melberg.Infrastructure.Rabbit.Factory;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Melberg.Infrastructure.Rabbit.Health;

public class RabbitPublisherHealthCheck<TMessage> : HealthCheck
{
    private readonly IConnection _connection;
    public RabbitPublisherHealthCheck(IServiceProvider serviceProvider)
    {
        _connection = serviceProvider.GetService<IStandardConnectionFactory>().GetPublisherChannel(typeof(TMessage).Name);
    }
    public override string Name => typeof(TMessage).Name+"_rabbitpublisher";
    public override Task<bool> IsOk(CancellationToken token)
    {
        return Task.FromResult(_connection.IsOpen);
    }
}