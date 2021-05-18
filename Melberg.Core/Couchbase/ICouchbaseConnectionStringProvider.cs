namespace Melberg.Core.Couchbase
{
    public interface ICouchbaseConnectionStringProvider
    {
        CouchbaseConfiguration GetConfiguration(string bucketName);
    }
}