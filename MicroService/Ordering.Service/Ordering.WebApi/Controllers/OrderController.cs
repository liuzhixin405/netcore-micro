using Grpc.Net.Client;
using GrpcService.CustomerService;
using MagicOnion.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Domain.Dtos;
using Ordering.Domain.Orders;
using Ordering.IGrain;
using Ordering.WebApi.Filters;
using Orleans;

namespace Ordering.WebApi.Controllers;


[AsyncAuthorizationFilter]
[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IGrainFactory _clusterClient;

    public OrderController(ILogger<OrderController> logger, IGrainFactory clusterClient)
    {
        _logger = logger;
        _clusterClient = clusterClient;
    }

    [HttpPost]
    [Route("Create")]
    
    public async Task<bool> Create(CreateOrderDto orderDto)
    {
        if(orderDto.pid == 0 || orderDto.price==0||orderDto.quantity==0)
        {
            return false;
        }
        var orderGrain = _clusterClient.GetGrain<IOrderGrain>(Random.Shared.Next());

        return await orderGrain.Create(orderDto);
    }

    [HttpGet]
    [Route("GetOrders")]
    public async Task<List<OrderDetailDto>> GetOrdersByUser()
    {
        var orderGrain = _clusterClient.GetGrain<IOrderGrain>(Random.Shared.Next());
        return await orderGrain.GetOrderDetails();
    }
}
