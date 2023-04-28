using System.Threading;
using System.Threading.Tasks;

namespace MelbergFramework.Core.Health;

public abstract class HealthCheck : IHealthCheck
{
    public string Reason;

    public abstract string Name {get;}

    public abstract Task<bool> IsOk(CancellationToken token);

}