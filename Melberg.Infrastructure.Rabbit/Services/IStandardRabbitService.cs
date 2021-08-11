using System.Threading.Tasks;

namespace Melberg.Infrastructure.Rabbit.Services
{
    public interface IStandardRabbitService
    {
        Task Run();
    }
}