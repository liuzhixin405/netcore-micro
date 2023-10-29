using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogs.Domain.Catalogs;
using Microsoft.EntityFrameworkCore;

namespace Catalogs.Infrastructure.Database
{
    public class CatalogContext:DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options):base(options) 
        {
                
        }

        public DbSet<Catalog> Catalogs { get; set; }
    }
}
