using Grpc.Net.Client;
using GrpcService.CustomerService;
using MagicOnion.Client;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ordering.WebApi.Filters
{
    public class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // 获取请求头中的 Authorization 值
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                // 如果 Authorization 不存在，拒绝请求
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                return;
            }

            // 从 Authorization 中提取 Token
            var token = authorizationHeader.ToString().Replace("Bearer ", string.Empty);
            var userId = 0L;
            // 在实际项目中，你需要调用相应的服务来解析 Token，并获取用户信息
            using (var channel = GrpcChannel.ForAddress("http://localhost:7005"))
            {
                var client = MagicOnionClient.Create<IGrpcCustomerService>(channel);
                userId = (client.GetUseIdFromToken(token)).GetAwaiter().GetResult().id;
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