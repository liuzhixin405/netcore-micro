using EfCoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCoreProject.Context
{
    public abstract class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext(DbContextOptions contextOptions, IConfiguration configuration) : base(contextOptions)
        {
            _configuration = configuration;
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (_configuration["DbType"]?.ToLower() == "sqlserver")
                modelBuilder.Entity<Product>()
       .Property<byte[]>("Version")
       .IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }
    }

    public class WriteProductDbContext : ApplicationDbContext
    {

        public WriteProductDbContext(DbContextOptions<WriteProductDbContext> options, IConfiguration configuration) : base(options, configuration)
        {
        }

        public Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }

    public class ReadProductDbContext : ApplicationDbContext
    {

        public ReadProductDbContext(DbContextOptions<ReadProductDbContext> options, IConfiguration configuration) : base(options, configuration)
        {
        }
    }
}
