using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Domain.OutBoxMessage
{
    [Table("OutboxMessageConsumer")]
    public class OutboxMessageConsumer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // 指定非自增

        public long Id { get; set; }
        public string Name { get; set; }
        public string? Error { get; set; }
    }
}
