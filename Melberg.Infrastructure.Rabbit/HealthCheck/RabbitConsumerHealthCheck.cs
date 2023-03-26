using System;
using System.Threading;
using System.Threading.Tasks;
using Melberg.Core.Health;
using Melberg.Infrastructure.Rabbit.Factory;
using Melberg.Infrastructure.Rabbit.Messages;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Melberg.Infrastructure.Rabbit.Health;

public class RabbitConsumerHealthCheck : HealthCheck
{
    private readonly IModel _connection;
    public RabbitConsumerHealthCheck(IServiceProvider serviceProvider)
    {
        _connection = serviceProvider.GetService<IStandardConnectionFactory>().GetConsumerModel();
    }

    public override string Name => "rabbitconsumer";
    public override Task<bool> IsOk(CancellationToken token)
    {
        return Task.FromResult(_connection.IsOpen);    
    }
}