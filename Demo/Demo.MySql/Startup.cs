using Demo.MySql.Infrastructure;
using Melberg.Application;
using Melberg.Core;
using Melberg.Infrastructure.MySql;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.MySql;

public class Startup : IAppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        MySqlModule.LoadSqlRepository<ITestRepository,TestRepository,ReadWriteContext>(services);
    }
}