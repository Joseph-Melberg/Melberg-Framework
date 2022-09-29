using System.Threading;
using System.Threading.Tasks;
using Melberg.Infrastructure.Rabbit.Messages;

namespace Melberg.Infrastructure.Rabbit.Consumers;
public interface IStandardConsumer
{
    Task ConsumeMessageAsync(Message message);
}