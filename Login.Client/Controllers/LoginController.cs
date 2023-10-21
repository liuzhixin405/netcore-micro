using System.ComponentModel.DataAnnotations;
using System.Threading.Channels;
using Grpc.Net.Client;
using MagicOnion.Client;
using MicroService.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Login.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {


        private readonly ILogger<LoginController> _logger;
        private IConfiguration _configuration;
        public LoginController(ILogger<LoginController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet(Name = "Login")]
        public async Task<ActionResult<Tuple<bool,string?>>> Login([Required]string user,[Required]string pwd)
        {
            SignInResponse authResult;
            using (var channel = GrpcChannel.ForAddress(_configuration["JwtAuthApp.ServiceAddress"])) 
            {
                var accountClient = MagicOnionClient.Create<IAccountService>(channel);
                 authResult = await accountClient.SignInAsync(user, pwd);
            }
            return (authResult!=null && authResult.Success)?  Tuple.Create(true,authResult.Token): Tuple.Create(false,string.Empty);
        }
    }
}