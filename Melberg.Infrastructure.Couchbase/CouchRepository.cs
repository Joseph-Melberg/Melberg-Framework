using System.Threading.Tasks;
using Couchbase.KeyValue;

namespace Melberg.Infrastructure.Couchbase;
public class CouchRepository 
{
    protected ICouchbaseCollection Collection;
    
    public CouchRepository(ICouchClientFactory couchClientFactory, string bucketName)
    {
        Task.Run(async () =>
        {
            Collection = await couchClientFactory.GenerateCollectionConnectionAsync(bucketName);
        }).Wait();
    }
}