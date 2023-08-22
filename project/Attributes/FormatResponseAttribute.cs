using Common.Util.Primitives;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NuGet.Protocol;
using project.Filters;

namespace project.Attributes
{
    public class FormatResponseAttribute:BaseAcyionFilter
    {
        protected override Task OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ContainsFilter<NoFormatResponseAttribute>())
                return Task.CompletedTask;
            if (context.Result is EmptyResult)
                context.Result = Success();
            else if(context.Result is ObjectResult res)
            {
                if (res.Value is AjaxResult)
                    context.Result = JsonContent(res.Value.ToJson());
                else
                    context.Result = Success(res.Value);
            }
            return Task.CompletedTask;
        }
    }
}
