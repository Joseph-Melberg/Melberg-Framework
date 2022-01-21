namespace Melberg.Core.Rabbit.Configurations.Data;
public class ExchangeConfigData
{
    public string Name {get; set;}     

    public string Connection {get; set;}

    public ExchangeConfigType Type {get; set;}

    public bool AutoDelete {get; set;}

    public bool Durable {get; set;}
    
}