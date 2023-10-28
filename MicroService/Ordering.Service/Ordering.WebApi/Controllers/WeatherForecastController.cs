using Grpc.Net.Client;
using GrpcService.CustomerService;
using MagicOnion.Client;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<string> GetUserTest(string username, string password)
    {
        using (var channel = GrpcChannel.ForAddress("https://localhost:7021"))
        {
            var client = MagicOnionClient.Create<IGrpcCustomerService>(channel);
            var result = await client.GetCustomer(new CustomerRequest(username,password));
            return result.username;
        }
    }
}
