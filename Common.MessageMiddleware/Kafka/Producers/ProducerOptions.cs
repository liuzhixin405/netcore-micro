using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageMiddleware.Kafka.Producers
{
    public class ProducerOptions : BaseOptions
    {
        /// <summary>
        /// 键值
        /// </summary>
        public string Key { get; set; }
    }
}
