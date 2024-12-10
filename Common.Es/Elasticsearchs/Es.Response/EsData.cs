using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Es
{
    public class EsData<T>
    {
        public long Total { get; set; }
        public List<T> List { get; set; }
    }
}
