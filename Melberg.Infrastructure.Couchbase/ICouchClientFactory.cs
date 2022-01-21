using System.Threading.Tasks;
using Couchbase.KeyValue;

namespace Melberg.Infrastructure.Couchbase;
public interface ICouchClientFactory
{
    Task<ICouchbaseCollection> GenerateCollectionConnectionAsync(string bucketName);
}