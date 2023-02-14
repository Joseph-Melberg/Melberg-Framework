using System.Threading.Tasks;

namespace Melberg.Core.Health;

public interface IHealthCheckChecker
{
    Task<bool> IsOk();
}