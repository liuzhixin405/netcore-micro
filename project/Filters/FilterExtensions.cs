using Microsoft.AspNetCore.Mvc.Filters;

namespace project.Filters
{
    public static class FilterExtensions
    {
        public static bool ContainsFilter<T>(this FilterContext actionExecutingContext)
        {
            return actionExecutingContext.Filters.Any(x=>x.GetType() == typeof(T));
        }
    }
}
