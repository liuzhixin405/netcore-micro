using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ordering.Domain.Dtos
{
    [Serializable]
    public class CatalogCopy
    {
        public long Id { get;  set; }
        public string Name { get;  set; }
        public string Description { get;  set; }
        public decimal Price { get;  set; }
        public int Stock { get;  set; }
        public int MaxStock { get;  set; } = 10000;
        public string ImgPath { get; set; }

    }
}
