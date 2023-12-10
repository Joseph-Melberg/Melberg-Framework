namespace Demo.RabbitConsumerWithMetric.Service;

public interface IThing
{
    int Value {get; set;}
}

public class Thing : IThing
{
    public Thing()
    {
        Value = (int)new Random().NextInt64();
    }
    public int Value {get; set;}
}