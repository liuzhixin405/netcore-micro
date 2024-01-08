using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogs.Domain.Catalogs;
using Catalogs.Domain.OutBoxMessage;
using Microsoft.EntityFrameworkCore;
using Catalogs.Domain.OutBoxMessage;

namespace Catalogs.Infrastructure.Database
{
    public class CatalogContext:DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options):base(options) 
        {
                
        }

        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<OutboxMessageConsumer> OutBoxMessageConsumers { get; set; }
        public DbSet<OutBoxMessage> OutBoxMessages { get; set; }
    }
}
