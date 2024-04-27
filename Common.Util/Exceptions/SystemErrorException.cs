using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util.Exceptions
{
    public class SystemErrorException : CommonException
    {
        public SystemErrorException(string msg="内部错误,请联系管理员", int code=500) : base(msg, code)
        {

        }
    }
}
