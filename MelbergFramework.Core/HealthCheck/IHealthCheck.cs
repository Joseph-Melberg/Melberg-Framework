using System.Threading;
using System.Threading.Tasks;

namespace MelbergFramework.Core.Health;

public interface IHealthCheck
{
    Task<bool> IsOk(CancellationToken token);
    string Name {get;}
}