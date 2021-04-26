using System.Threading;
using System.Threading.Tasks;
using Melberg.Core.Messaging;
using Melberg.Infrastructure.Rabbit.Models;

namespace Melberg.Infrastructure.Rabbit.Consumers
{
    public interface IMessageProcessor
        {
        Task<MessageProcessingResult> ProcessMessage(Message message, CancellationToken cancellationToken);
        Task<MessageProcessingResult> HandleException(System.Exception ex, CancellationToken cancellationToken);
    }
}