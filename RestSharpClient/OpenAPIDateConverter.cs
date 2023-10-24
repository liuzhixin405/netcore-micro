using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpComponent
{
    public class OpenAPIDateConverter:IsoDateTimeConverter
    {
        public OpenAPIDateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
