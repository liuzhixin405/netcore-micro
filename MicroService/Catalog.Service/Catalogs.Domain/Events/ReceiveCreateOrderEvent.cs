using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Domain.Events
{
    public class ReceiveCreateOrderEvent
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
    }
}
