using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util.Exceptions
{
    public abstract class CommonException:Exception
    {
        public int Code;
        public string Msg;
      
        public CommonException(string message="",int code=0)
        {
            this.Msg = message;
            this.Code = code;
        }

    }
}
