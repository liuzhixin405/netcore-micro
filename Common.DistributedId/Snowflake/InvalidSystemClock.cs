using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DistributedId
{
    internal class InvalidSystemClock:Exception
    {
        public InvalidSystemClock(string message):base(message) { }
    }
}
