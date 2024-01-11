using Microsoft.EntityFrameworkCore;

namespace Paying.WebApi.Database
{
    public class PaymentContext:DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }
     
    }
}
