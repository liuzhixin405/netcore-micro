using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util.Exceptions
{
    public class ParamsErrorException : CommonException
    {
        public ParamsErrorException(string msg="参数错误",int code=400):base(msg,code)
        {
            
        }
    }
}
