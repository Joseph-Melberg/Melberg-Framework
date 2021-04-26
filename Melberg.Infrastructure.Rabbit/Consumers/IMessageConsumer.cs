using System;
using System.Threading;
using System.Threading.Tasks;
using Melberg.Infrastructure.Rabbit.Connection;
using Melberg.Infrastructure.Rabbit.Models;

namespace Melberg.Infrastructure.Rabbit.Consumers
{
    public interface IMessageConsumer : IDisposable
    {
        Task ConsumeMessageAsync(Message message, IChannel channel, CancellationToken cancellationToken);
    }
}