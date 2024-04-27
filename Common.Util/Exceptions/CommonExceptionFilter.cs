using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Common.Util.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Common.Util.Exceptions
{
    public class CommonExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger _logger;
        public CommonExceptionFilter(ILogger<CommonExceptionFilter> logger)
        {
            _logger = logger;
        }
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
              Exception ex = context.Exception;
                if(ex is SystemErrorException cex)
                {
                    _logger.LogError(cex.Message);   //写日志
                    context.Result = Error(cex.Msg, cex.Code);   
                }
                if(ex is ParamsErrorException pex)
                {
                    context.Result = Error(pex.Msg, pex.Code);
                }
            }
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }

        private ContentResult Error(string message,int code)
        {
            return new ContentResult() { Content = message, StatusCode = code, ContentType = "application/json; charset=utf-8" };
        }


    }
}
