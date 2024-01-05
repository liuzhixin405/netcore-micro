using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpComponent
{
    public class ApiException:Exception
    {
        public int ErrorCode { get; set; }
        public object ErrorContent { get; set; }
        public ApiException()
        {

        }
        public ApiException(int errorCode,string message):base(message)
        {
            this.ErrorCode = errorCode;
        }
        public ApiException(int errorCode,string message,object errorContent = null):base(message)
        {
            this.ErrorCode = errorCode;
            this.ErrorContent = errorContent;

        }
    }
}
