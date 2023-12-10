using System;
using System.Threading;
using System.Threading.Tasks;
using MelbergFramework.Core.Health;
using MelbergFramework.Infrastructure.Rabbit.Factory;
using MelbergFramework.Infrastructure.Rabbit.Messages;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MelbergFramework.Infrastructure.Rabbit.Health;

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
