using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.Dimain.OutBoxMessages
{
    [Table("OutBoxMessage")]
    public class OutBoxMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // 指定非自增
        public long Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public DateTime OccurredOnUtc { get; set; }
        public DateTime? ProceddedOnUtc { get; set; }
        public string? Error { get; set; }
    }
}
