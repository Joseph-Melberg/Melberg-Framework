using Demo.InfluxDB.Infrastructure;
using Demo.InfluxDB.Infrastructure.Core;
using MelbergFramework.Application;
using MelbergFramework.Infrastructure.InfluxDB;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.InfluxDB;

public class Startup : IAppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        InfluxDBModule.LoadInfluxDBRepository<ITestRepo,TestRepo,TestInfluxDBContext>(services);
    }
}