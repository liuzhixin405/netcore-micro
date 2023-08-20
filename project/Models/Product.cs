using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long CreateTime { get; set; }
        [NotMapped] //mysql不支持
        public byte[] Version { get; set; }
    }

}
