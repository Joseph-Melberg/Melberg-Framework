using System.Threading;
using System.Threading.Tasks;
using Melberg.Core.Messaging;
using Melberg.Infrastructure.Rabbit.Models;
using Microsoft.Extensions.Logging;

namespace Melberg.Infrastructure.Rabbit.Consumers
{
    public abstract class BaseServiceProcessor : IMessageProcessor
    {

        protected BaseServiceProcessor()
        {
        }

        public virtual Task<MessageProcessingResult> HandleException(System.Exception ex, CancellationToken cancellationToken)
        {
            return Task.FromResult(MessageProcessingResult.Failed);
        }

        public abstract Task<MessageProcessingResult> ProcessMessage(Message message, CancellationToken cancellationToken);
    }
}