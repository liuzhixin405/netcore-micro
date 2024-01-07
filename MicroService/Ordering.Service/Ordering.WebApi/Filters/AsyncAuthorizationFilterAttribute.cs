using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using GrpcService.CustomerService;
using MagicOnion.Client;
using System.Net;

namespace Ordering.WebApi.Filters
{
    public class AsyncAuthorizationFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            if (context.ActionDescriptor.EndpointMetadata.Any(x => x is AllowAnonymousAttribute))
            {
                return;
            }
            else
            {
                string token = context.HttpContext.Request.Headers["Token"];

                if (string.IsNullOrEmpty(token))
                {
                   // 如果 Authorization 不存在，拒绝请求
                    context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                    return;
                }

                // 从 Authorization 中提取 Token
                var userId = 0L;
                // 在实际项目中，你需要调用相应的服务来解析 Token，并获取用户信息
                using (var channel = GrpcChannel.ForAddress("https://localhost:7021"))
                {
                    try
                    {
                        var client = MagicOnionClient.Create<IGrpcCustomerService>(channel);
                        userId =(await client.GetUseIdFromToken(token)).id;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"获取用户信息出错:{ex.Message}");
                    }
                }
                if (userId == 0L)
                {
                    // 如果用户信息无效，拒绝请求
                    context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                    throw new Exception("授权错误");
                }
                // 将用户信息传递给 Controller 或 Action
                context.HttpContext.Items["UserId"] = userId;
            }
        }
    }
}
