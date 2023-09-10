using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Events
{
    public class OrderCompleteDomainEvent: INotification
    {
        public Order Order { get; }

        public OrderCompleteDomainEvent(Order order)
        {
            Order = order;
        }
    }
}
