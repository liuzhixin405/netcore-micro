using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Enums;

namespace Ordering.Domain.Dtos
{
    public class OrderDetailDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string OrderStatus { get; set; }
        public Decimal TotalAmount { get; set; }
        public string CreateTime { get;  set; }
    }
}
