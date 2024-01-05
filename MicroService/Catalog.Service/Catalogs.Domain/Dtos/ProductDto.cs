using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalogs.Domain.Dtos
{
    public class ProductDto
    {
        [JsonPropertyName("Id")]
        public string Id { get; private set; }

        [JsonPropertyName("Name")]
        public string Name { get; private set; }

        [JsonPropertyName("Price")]
        public string Price { get; private set; }

        [JsonPropertyName("Stock")]
        public string Stock { get; private set; }

        [JsonPropertyName("ImgPath")]
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
