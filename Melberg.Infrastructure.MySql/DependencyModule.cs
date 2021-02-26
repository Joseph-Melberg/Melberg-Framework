using Microsoft.Extensions.DependencyInjection;

namespace Melberg.Infrastructure.MySql
{
    public class MySqlModule
    {
        public static void LoadSqlRepository<TFrom, TTo, TContext>(IServiceCollection catalog)
            where TTo : BaseRepository<TContext>,TFrom
            where TFrom : class
            where TContext : DefaultContext
        {
            catalog.AddSingleton<IMySQLConnectionStringProvider, MySQLConnectionStringProvider>();

            catalog.AddTransient<TFrom, TTo>();

            catalog.AddTransient<TContext, TContext>();
        }
    }
}