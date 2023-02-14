using Melberg.Core.Health;
using Melberg.Core.MySql;
using Melberg.Infrastructure.MySql.Health;
using Microsoft.Extensions.DependencyInjection;

namespace Melberg.Infrastructure.MySql;
public class MySqlModule
{
    public static void LoadSqlRepository<TFrom, TTo, TContext>(IServiceCollection catalog)
        where TTo : BaseRepository<TContext>,TFrom
        where TFrom : class
        where TContext : DefaultContext
    {
        catalog.AddSingleton<IMySqlConnectionStringProvider, MySQLConnectionStringProvider>();

        catalog.AddTransient<TFrom, TTo>();

        catalog.AddTransient<TContext, TContext>();

        catalog.AddTransient<IHealthCheck,MySqlHealthCheck<TContext>>();
    }
}