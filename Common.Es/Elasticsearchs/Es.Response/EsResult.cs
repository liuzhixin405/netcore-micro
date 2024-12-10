using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Es
{
    public class EsResult
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public object Metadata { get; set; }
    }
}
