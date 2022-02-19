using Melberg.Infrastructure.Rabbit.Messages;

namespace Melberg.Infrastructure.Rabbit.Publishers;

public interface IStandardPublisher<TMessage>
    where TMessage : IStandardMessage
{
    void Send(TMessage message);
}