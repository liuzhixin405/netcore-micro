using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Dtos
{
    public record CreateOrderDto(long pid, int quantity, decimal price)
    {
    }
}
