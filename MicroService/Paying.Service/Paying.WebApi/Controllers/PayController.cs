using System.Text;
using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;
using Paying.WebApi.Dtos;
using Paying.WebApi.Services;
using RabbitMQ.Client;

namespace Paying.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PayController : ControllerBase
{

  
    private readonly ILogger<PayController> _logger;
    private readonly IPayingService _payingService;
    public PayController(ILogger<PayController> logger,IPayingService payingService)
    {
        _logger = logger;
        _payingService = payingService;
    }


    [HttpPost]
    [Route("Paying")]
    public async Task<bool> Paying(long orderId,decimal amount)
    {
       await _payingService.ChangeOrderStatus(orderId,6);
        //支付测试,无实际业务
        return true;
    }

}
