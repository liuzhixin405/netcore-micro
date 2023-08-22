using System;
using Microsoft.AspNetCore.Mvc;
using project.Attributes;
using project.Models.Common;

namespace project.Utility.BaseController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [FormatResponse]
    public abstract class AbsEfWorkController<T> : ControllerBase where T : IEntity
    {
        //分页格式待定
        [HttpGet("Search/{id}")]
        [HttpPut("Update")]  //参数在body  swagger无法做测试
        [HttpPost("Add")]   //参数在body
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Invoke()
        {
            var request = HttpContext.Request;
            int id = 0;
            var mtype = (MethodType)Enum.Parse(typeof(MethodType), request.Method, true);
            switch (mtype)
            {
                case MethodType.GET:
                    id = int.Parse(request.RouteValues["id"].ToString());
                    return await Search(id);
                case MethodType.POST:
                case MethodType.PUT:
                    T product;
                    using (var reader = new StreamReader(HttpContext.Request.Body))
                    {
                        var json = reader.ReadToEnd();
                        product = System.Text.Json.JsonSerializer.Deserialize<T>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        // 处理 myModel 对象...
                    }
                    return await CreateOrUpdate(product);
                //update or add
                case MethodType.DELETE:
                    id = int.Parse(request.RouteValues["id"].ToString());
                    return await Delete(id);
                default:
                    throw new Exception("未知");
            }
            await Task.CompletedTask;
            return Ok();
        }
        protected abstract Task<IActionResult> Search<KeyType>(KeyType searchId);



        protected abstract Task<IActionResult> CreateOrUpdate(T product);



        protected abstract Task<IActionResult> Delete<KeyType>(KeyType entity);

        protected abstract Task<IActionResult> PageResult();

    }
}

