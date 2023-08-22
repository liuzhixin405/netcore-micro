using Microsoft.AspNetCore.Mvc.Filters;

namespace project.Attributes
{
    public class NoFormatResponseAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
