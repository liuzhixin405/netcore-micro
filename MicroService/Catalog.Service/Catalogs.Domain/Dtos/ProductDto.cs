using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Domain.Dtos
{
    public class ProductDto
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Price { get; private set; }
        public string Stock { get; private set; }
        public string ImgPath { get; private set; }
        public ProductDto(string id,string name,string price,string stock,string imgPath)
        {
            Id= id;
            Name= name;
            Price = price;
            Stock = stock;
            ImgPath= imgPath;

        }
    }
}
