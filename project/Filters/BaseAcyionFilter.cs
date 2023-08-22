using System.Text.Json.Serialization;
using Common.Util.Primitives;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace project.Filters
{
    public class BaseAcyionFilter : Attribute, IAsyncActionFilter
    {
        protected virtual Task OnActionExecuting(ActionExecutingContext context) => Task.CompletedTask;
        protected virtual Task OnActionExecuted(ActionExecutedContext context) => Task.CompletedTask;
        async Task IAsyncActionFilter.OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await OnActionExecuting(context);
            if(context.Result == null)
            {
                var nextContext = await next();
                await OnActionExecuted(nextContext);
            }
        }

        public ContentResult JsonContent(string json)=> new ContentResult { Content = json, StatusCode = 200, ContentType = "application/json;charset=utf-8" };

        public ContentResult Success() => JsonContent(System.Text.Json.JsonSerializer.Serialize(new AjaxResult { Success = true, Msg = "请求成功!" }));
        public ContentResult Success(string msg) => JsonContent(System.Text.Json.JsonSerializer.Serialize(new AjaxResult { Success = true, Msg = msg }));
        public ContentResult Success<T>(T obj) => JsonContent(System.Text.Json.JsonSerializer.Serialize(new AjaxResult<T> { Success = true, Msg = "请求成功!", Data = obj }));


        public ContentResult Error() => JsonContent(System.Text.Json.JsonSerializer.Serialize(new AjaxResult { Success = false, Msg = "请求失败!" }));
        public ContentResult Error(string msg) => JsonContent(System.Text.Json.JsonSerializer.Serialize(new AjaxResult { Success = false, Msg = msg }));
        public ContentResult Error<T>(string msg,int errorCode) => JsonContent(System.Text.Json.JsonSerializer.Serialize(new AjaxResult<T> { Success = false, Msg = msg, ErrorCOde = errorCode }));
    }
}
