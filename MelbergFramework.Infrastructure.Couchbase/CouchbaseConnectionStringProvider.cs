using MelbergFramework.Core.Couchbase;
using Microsoft.Extensions.Configuration;

namespace MelbergFramework.Infrastructure.Couchbase;
public class CouchbaseConnectionStringProvider : ICouchbaseConnectionStringProvider
{
    private readonly IConfiguration _configuration;

    public CouchbaseConnectionStringProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public CouchbaseConfiguration GetConfiguration(string bucketName)
    {
        var url = _configuration.GetSection($"Couchbase:{bucketName}:Url").Value;
        var username = _configuration.GetSection($"Couchbase:{bucketName}:Username").Value;
        var password = _configuration.GetSection($"Couchbase:{bucketName}:Password").Value;
        return new CouchbaseConfiguration
        {
            Password = password,
            Url = url,
            Username = username
        };
    }
}