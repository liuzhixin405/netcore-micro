using Common.Util.Primitives;
using Microsoft.AspNetCore.Mvc.Filters;

namespace project.Filters
{
    public class GlobalExceptionFilter : BaseActionFilter, IAsyncExceptionFilter
    {
        private readonly ILogger _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
                _logger = logger;
        }
        public Task OnExceptionAsync(ExceptionContext context)
        {
            Exception ex = context.Exception;
            if(ex is ShowException showEx)
            {
                _logger.LogInformation(showEx.Message);
            }
            else
            {
                _logger.LogError(ex,"");
                context.Result = Error("系统繁忙");
            }
            return Task.CompletedTask;
        }
    }
}
