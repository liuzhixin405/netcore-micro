using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Customers.Domain.Customers;

namespace Customers.Infrastructure.Database
{
    public class CustomerContext:DbContext
    {
        public CustomerContext(DbContextOptions options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerContext).Assembly);
        }
    }
}
