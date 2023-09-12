using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using project.Models.Common;

namespace project.Models
{
    [Table("Product")]
    public class Product:IEntity
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long CreateTime { get; set; }
        [NotMapped] //mysql不支持
        public byte[] Version { get; set; }
    }

}
