using Grpc.Net.Client;
using GrpcService.CustomerService;
using MagicOnion.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Domain.Dtos;
using Ordering.WebApi.Services;

namespace Ordering.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;

    public OrderController(ILogger<OrderController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<bool> Add([FromBody]CreateOrderDto orderDto)
    {
        return await _orderService.Create(orderDto);
    }
    //[HttpGet]
    //public async Task<string> GetUserTest(string username, string password)
    //{
    //    using (var channel = GrpcChannel.ForAddress("https://localhost:7021"))
    //    {
    //        var client = MagicOnionClient.Create<IGrpcCustomerService>(channel);
    //        var result = await client.GetCustomer(new CustomerRequest(username,password));
    //        return result.username;
    //    }
    //}
}
