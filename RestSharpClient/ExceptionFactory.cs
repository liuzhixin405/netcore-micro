using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpComponent
{
    /// <summary>
    /// A delegate to ExceptionFactory method
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    public delegate Exception ExceptionFactory(string methodName, IApiResponse response);
}
