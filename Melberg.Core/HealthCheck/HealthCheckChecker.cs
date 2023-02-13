using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Melberg.Core.Health;


public class HealthCheckChecker : IHealthCheckChecker
{
    private readonly IEnumerable<IHealthCheck> _healthsCheck;
    public HealthCheckChecker(IEnumerable<IHealthCheck> healthsCheck)
    {
        //I will make a better system, this weeds out the duplicates
        _healthsCheck = healthsCheck.DistinctBy(_ => _.Name); 
    }
    public async Task<bool> IsOk()
    {
        var waitTask = Task.Delay(5000);

        var healths = _healthsCheck.Select(_ => _.IsOk(System.Threading.CancellationToken.None));
        var healthsCheckTask = Task.WhenAll(healths);

        await Task.WhenAny(waitTask,healthsCheckTask);

        if(waitTask.IsCompletedSuccessfully)
        {
            return false;
        }

        return !healthsCheckTask.Result.Any(_ => !_);

    }
}