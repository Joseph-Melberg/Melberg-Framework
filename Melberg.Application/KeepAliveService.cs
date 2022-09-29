using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Melberg.Application;

public class KeepAliveService : IHostedService
{
    public KeepAliveService() { }


    public Task StartAsync(CancellationToken cancellationToken)
    {
       return Task.Delay(-1,cancellationToken); 
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}