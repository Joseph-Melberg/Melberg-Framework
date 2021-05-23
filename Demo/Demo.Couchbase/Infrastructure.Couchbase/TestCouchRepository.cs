using Melberg.Infrastructure.Couchbase;

namespace Demo.Couchbase.Infrastructure.Couchbase
{
    public class TestCouchRepository: CouchRepository, ITestCouchRepository 
    {
        public TestCouchRepository(ICouchClientFactory clientFactory) : base(clientFactory,"test")
        {

        }
    }
}