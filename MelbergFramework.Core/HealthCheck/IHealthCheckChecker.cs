using System.Threading.Tasks;

namespace MelbergFramework.Core.Health;

public interface IHealthCheckChecker
{
    Task<bool> IsOk();
}