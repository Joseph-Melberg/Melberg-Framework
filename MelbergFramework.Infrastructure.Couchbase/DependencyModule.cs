using MelbergFramework.Core.Couchbase;
using Microsoft.Extensions.DependencyInjection;

namespace MelbergFramework.Infrastructure.Couchbase;
public class CouchbaseModule
{
    public static void RegisterCouchbaseClient<TFrom,TTo>(IServiceCollection catalog)
    where TTo : CouchRepository, TFrom
    where TFrom : class
    {
        catalog.AddTransient<ICouchClientFactory,CouchClientFactory>();
        catalog.AddTransient<TFrom,TTo>();
        catalog.AddTransient<ICouchbaseConnectionStringProvider,CouchbaseConnectionStringProvider>();
    }
}