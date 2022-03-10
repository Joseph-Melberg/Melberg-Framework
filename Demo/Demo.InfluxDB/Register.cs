using Demo.InfluxDB.Infrastructure;
using Demo.InfluxDB.Infrastructure.Core;
using Melberg.Infrastructure.InfluxDB;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.InfluxDB
{
    public class Register
    {

        public static ServiceCollection RegisterServices(ServiceCollection services)
        {
            InfluxDBModule.LoadInfluxDBRepository<ITestRepo,TestRepo,TestInfluxDBContext>(services);
            return services;
        }
    }
}