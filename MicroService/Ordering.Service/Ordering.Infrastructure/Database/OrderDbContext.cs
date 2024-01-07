
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ordering.Domain.Orders;

namespace Ordering.Infrastructure.Database
{
    public abstract class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext(DbContextOptions contextOptions, IConfiguration configuration) : base(contextOptions)
        {
            _configuration = configuration;
        }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (_configuration["DbType"]?.ToLower() == "sqlserver")
                modelBuilder.Entity<Order>()
       .Property<byte[]>("Version")
       .IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }
    }

    public class WriteOrderDbContext : ApplicationDbContext
    {

        public WriteOrderDbContext(DbContextOptions<WriteOrderDbContext> options, IConfiguration configuration) : base(options, configuration)
        {
        }

        public Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }

    public class ReadOrderDbContext : ApplicationDbContext
    {

        public ReadOrderDbContext(DbContextOptions<ReadOrderDbContext> options, IConfiguration configuration) : base(options, configuration)
        {
        }
    }
}
