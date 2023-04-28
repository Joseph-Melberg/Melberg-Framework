using MelbergFramework.Infrastructure.Rabbit.Messages;

namespace MelbergFramework.Infrastructure.Rabbit.Publishers;

public interface IStandardPublisher<TMessage>
    where TMessage : IStandardMessage
{
    void Send(TMessage message);
}