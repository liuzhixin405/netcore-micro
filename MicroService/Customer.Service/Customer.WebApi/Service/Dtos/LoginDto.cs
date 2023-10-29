using System.ComponentModel.DataAnnotations;

namespace Customers.Center.Service.Dtos
{
    
    public record LoginDto([Required]string username,[Required]string password);
    
}
