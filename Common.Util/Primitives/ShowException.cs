using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util.Primitives
{
    /// <summary>
    /// 业务异常
    /// 不做异常处理,仅为方便返回前端错误提示信息
    /// </summary>
    public class ShowException:Exception
    {
        public int ErrorCode { get; set; }

        public ShowException()
        {
                
        }

        public ShowException(string message):base(message)
        {
            
        }

        public ShowException(string message,int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
