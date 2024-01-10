using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paying.WebApi.Dtos
{
    public class CreateOrderEvent
    {
        public long EventId { get;set; }
        public long UserId { get; private set; }
        public long ProductId { get; private set; }
        public int Quantity { get;private set; }
        public CreateOrderEvent(long EventId, long userId,long productId,int quantity)
        {
            this.EventId = EventId;
            UserId = userId;
            ProductId=productId;
            Quantity = quantity;
        }
    }
}
