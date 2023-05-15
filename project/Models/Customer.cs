using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long CreateTime { get; set; }  //不支持Version
    }
}
