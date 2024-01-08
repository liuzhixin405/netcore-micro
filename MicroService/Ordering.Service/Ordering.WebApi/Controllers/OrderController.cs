using Grpc.Net.Client;
using GrpcService.CustomerService;
using MagicOnion.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Domain.Dtos;
using Ordering.WebApi.Filters;
using Ordering.WebApi.Services;

namespace Ordering.WebApi.Controllers;


[AsyncAuthorizationFilter]
[ApiController]
[Route("[controller]")]
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
    [Route("Create")]
    
    public async Task<bool> Create(CreateOrderDto orderDto)
    {
        if(orderDto.pid == 0 || orderDto.price==0||orderDto.quantity==0)
        {
            return false;
        }
        return await _orderService.Create(orderDto);
    }
}
