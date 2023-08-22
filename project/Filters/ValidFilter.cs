using Microsoft.AspNetCore.Mvc.Filters;

namespace project.Filters
{
    public class ValidFilter:BaseAcyionFilter
    {
        protected override Task OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var list = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                context.Result = Error(string.Join(", ", list));
            }

            return Task.CompletedTask;
        }
    }
}
