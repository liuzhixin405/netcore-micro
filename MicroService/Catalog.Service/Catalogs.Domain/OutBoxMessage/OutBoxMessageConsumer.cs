using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Domain.OutBoxMessage
{
    [Table("OutboxMessageConsumer")]
    public class OutboxMessageConsumer
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
