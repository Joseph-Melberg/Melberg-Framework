namespace MelbergFramework.Core.Rabbit.Configurations.Data;
public class PublisherConfigData
{
    public string Name { get; set; }

    public string Connection { get; set; }

    public string Exchange { get; set; }

    public bool Mandatory { get; set; }

    public bool Immediate { get; set; }

    public string Type { get; set; }
}