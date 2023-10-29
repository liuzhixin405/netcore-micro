using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Domain.Catalogs
{
    [Table("Tables")]
    public  class Catalog
    {
        public static Catalog CreateNew(long id,string name,decimal price,int stock,int maxStock,string desc = "")
        {
            var catalog = new Catalog()
            {
                Id = id,
                Name = name,
                Price = price,
                Stock = stock,
                MaxStock = maxStock,
                Description = desc
            };
            return catalog;
        }
        [Key]
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public int MaxStock { get; private set; } = 10000;
        protected Catalog()
        {
            
        }
        public Task<Tuple<bool,string>> AddStock(int quantity)
        {

            if(Stock+quantity > MaxStock)
            {
                return Task.FromResult(Tuple.Create(false, "超出库存限制"));
            }
            Stock += quantity;
            return Task.FromResult(Tuple.Create(true,""));
        }
        public Task<Tuple<bool, string>> RemoveStock(int quantity)
        {

            if ( quantity > Stock)
            {
                return Task.FromResult(Tuple.Create(false, "超出库存限制"));
            }
            Stock -= quantity;
            return Task.FromResult(Tuple.Create(true, ""));
        }

    }
}
