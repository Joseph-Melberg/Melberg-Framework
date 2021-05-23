using Microsoft.Extensions.DependencyInjection;
using Melberg.Infrastructure.Couchbase;
using Demo.Couchbase.Infrastructure.Couchbase;
using Demo.Couchbase.Infrastructure;

namespace Demo.Couchbase
{
    public static class Register
    {
        public static ServiceCollection RegisterServices(ServiceCollection services)
        {
            CouchbaseModule.RegisterCouchbaseClient<ITestCouchRepository,TestCouchRepository>(services);
            return services;
        }
    }
}