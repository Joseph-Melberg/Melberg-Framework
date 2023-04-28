using System.Threading.Tasks;
using Couchbase.KeyValue;

namespace MelbergFramework.Infrastructure.Couchbase;
public interface ICouchClientFactory
{
    Task<ICouchbaseCollection> GenerateCollectionConnectionAsync(string bucketName);
}