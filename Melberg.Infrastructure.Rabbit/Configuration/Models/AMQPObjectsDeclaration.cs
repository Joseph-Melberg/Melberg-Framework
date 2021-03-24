using System.Collections.Generic;
using Melberg.Infrastructure.Rabbit.Models;

namespace Melberg.Infrastructure.Rabbit.Configuration.Models
{
    public class AmqpObjectsDeclaration
    {
        public AmqpObjectsDeclaration()
        {
            ExchangeList = new List<ExchangeDeclaration>();
            QueueList = new List<QueueDeclaration>();
            BindingList = new List<BindingDeclaration>();
        }
        public List<ExchangeDeclaration> ExchangeList;
        public List<QueueDeclaration> QueueList;
        public List<BindingDeclaration> BindingList;
    }
}