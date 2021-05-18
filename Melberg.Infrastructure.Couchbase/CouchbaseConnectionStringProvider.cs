using Melberg.Core.Couchbase;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Melberg.Infrastructure.Couchbase
{
    public class CouchbaseConnectionStringProvider : ICouchbaseConnectionStringProvider
    {
        private readonly IConfiguration _configuration;

        public CouchbaseConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CouchbaseConfiguration GetConfiguration(string bucketName)
        {
            var value = _configuration.GetSection("Couchbase").GetSection(bucketName).Value;
            var result = JsonConvert.DeserializeObject<CouchbaseConfiguration>(value);
            return result;
        }
    }
}