using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;

namespace Paying.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PayController : ControllerBase
{
    
    private readonly ILogger<PayController> _logger;

    public PayController(ILogger<PayController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Route("Paying")]
    public async Task<bool> Paying([FromServices]Channel<string> channel, decimal amount)
    {
       await channel.Writer.WriteAsync("pay");
        //支付测试,无实际业务
        return true;
    }
}
