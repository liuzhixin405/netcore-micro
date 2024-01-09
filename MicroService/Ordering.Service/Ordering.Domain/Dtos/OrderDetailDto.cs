using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Enums;
using Orleans;
using Orleans.CodeGeneration;

namespace Ordering.Domain.Dtos
{
    [GenerateSerializer]
    public record OrderDetailDto
    {
        [Id(0)]
        public string ProductName { get; set; }
        [Id(1)]
        public string Quantity { get; set; }
        [Id(2)]
        public string OrderStatus { get; set; }
        [Id(3)]
        public string TotalAmount { get; set; }
        [Id(4)]
        public string CreateTime { get;  set; }
    }
}
