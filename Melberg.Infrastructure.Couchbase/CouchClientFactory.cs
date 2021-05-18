using System.Threading.Tasks;
using Couchbase;
using Couchbase.KeyValue;
using Melberg.Core.Couchbase;

namespace Melberg.Infrastructure.Couchbase
{
    public class CouchClientFactory
    {
        private readonly ICouchbaseConnectionStringProvider _connectionStringProvider;

        public CouchClientFactory(ICouchbaseConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }
        
        public async Task<ICouchbaseCollection> GenerateCouchbaseClientAsync(string bucketName)
        {
            var config = _connectionStringProvider.GetConfiguration(bucketName);

            var connection = await Cluster.ConnectAsync(config.Url,config.Username,config.Password);

            var bucket = await connection.BucketAsync(bucketName);

            return await bucket.DefaultCollectionAsync();
        }
    }
}