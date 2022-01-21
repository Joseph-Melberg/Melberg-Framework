using System.Threading.Tasks;

namespace Melberg.Infrastructure.Rabbit.Consumers;
public interface IStandardConsumer
{
    Task ConsumeMessageAsync(string message);
    
}