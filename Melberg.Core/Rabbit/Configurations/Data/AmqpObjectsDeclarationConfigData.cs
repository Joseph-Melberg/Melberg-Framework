using System.Collections.Generic;

namespace Melberg.Core.Rabbit.Configurations.Data
{
    public class AmqpObjectsDeclarationConfigData
    {
        public AmqpObjectsDeclarationConfigData()
        {
            ExchangeList = new List<ExchangeConfigData>();

            QueueList = new List<QueueConfigData>();

            BindingList = new List<BindingConfigData>();
        }

    public IEnumerable<ExchangeConfigData> ExchangeList {get; set;}

    public IEnumerable<QueueConfigData> QueueList {get; set;}

    public IEnumerable<BindingConfigData> BindingList {get; set;}
    }
}