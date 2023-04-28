using Demo.MySql.Infrastructure;
using MelbergFramework.Application;
using MelbergFramework.Core;
using MelbergFramework.Infrastructure.MySql;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.MySql;

public class Startup : IAppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        MySqlModule.LoadSqlRepository<ITestRepository,TestRepository,ReadWriteContext>(services);
    }
}