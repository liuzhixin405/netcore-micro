using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Domain.Events
{
    public interface IDomainEvent
    {
        public long EventId { get; set; }
    }
}
