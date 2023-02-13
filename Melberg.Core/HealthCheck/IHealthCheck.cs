using System.Threading;
using System.Threading.Tasks;

namespace Melberg.Core.Health;

public interface IHealthCheck
{
    Task<bool> IsOk(CancellationToken token);
    string Name {get;}
}