using EfCoreProject.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCoreProject.Context
{
    public abstract class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions contextOptions) :base(contextOptions)
        {

        }
        public DbSet<Product> Products { get; set; }
    }

    public class WriteProductDbContext: ApplicationDbContext,IUnitOfWork
    {
      
        public WriteProductDbContext(DbContextOptions<WriteProductDbContext> options) : base(options)
        {
        }

        public Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }

    public class ReadProductDbContext : ApplicationDbContext
    {
       
        public ReadProductDbContext(DbContextOptions<ReadProductDbContext> options) : base(options)
        {
        }
    }
}
