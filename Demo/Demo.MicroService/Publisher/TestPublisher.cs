using MelbergFramework.Infrastructure.Rabbit.Publishers;

namespace Demo.Microservice.Publisher;

public interface IPublisherTest
{
    void Send(); 
}

public class PublisherTest : IPublisherTest
{
    private IStandardPublisher<TestMessage> _pub;
    
    public PublisherTest(IStandardPublisher<TestMessage> standardPublisher)
    {
        _pub = standardPublisher;    
    }

    public void Send()
    {
        _pub.Send(new TestMessage());
    }
}