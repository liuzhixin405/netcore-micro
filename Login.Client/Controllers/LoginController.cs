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
        private readonly GrpcChannel _grpcChannel;
        public LoginController(ILogger<LoginController> logger, IConfiguration configuration)
        {
            _grpcChannel = GrpcChannel.ForAddress(configuration["JwtAuthApp.ServiceAddress"]);
            _logger = logger;
        }

        [HttpGet(Name = "Login")]
        public async Task<ActionResult<Tuple<bool,string?>>> Login(string user,string pwd)
        {
            var accountClient = MagicOnionClient.Create<IAccountService>(_grpcChannel);
            var authResult = await accountClient.SignInAsync(user,pwd);
            
            return (authResult!=null && authResult.Success)?  Tuple.Create(true,authResult.Token): Tuple.Create(false,string.Empty);
        }
    }
}