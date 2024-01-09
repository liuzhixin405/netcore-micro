using System.ComponentModel.DataAnnotations;
using System.Threading.Channels;
using Grpc.Net.Client;
using Login.Client.GrpcClient;
using MagicOnion.Client;
using MicroService.Shared;
using MicroService.Shared.GrpcPool;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Login.Client.HtppClient.WeatherforecastApi;

namespace Login.Client.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoginController : ControllerBase
    {


        private readonly ILogger<LoginController> _logger;
        private IConfiguration _configuration;
        private readonly IGrpcClientFactory<IAccountService> _grpcClientFactory;
        private readonly GrpcClientPool<IAccountService> _grpcClientPool;
        private readonly IClusterClient _clusterClient;
        public LoginController(ILogger<LoginController> logger, IConfiguration configuration, IGrpcClientFactory<IAccountService> grpcClientFactory, GrpcClientPool<IAccountService> grpcClientPool, IClusterClient clusterClient)
        {

            _configuration = configuration;
            _logger = logger;
            _grpcClientFactory = grpcClientFactory;
            _grpcClientPool = grpcClientPool;
            _clusterClient = clusterClient;
        }

        [HttpGet(Name = "Login")]
        public async Task<ActionResult<Tuple<bool,string?>>> Login([Required]string signInId, [Required]string pwd)
        {
            SignInResponse authResult;
            /*using (var channel = GrpcChannel.ForAddress(_configuration["JwtAuthApp.ServiceAddress"])) 
            {
                //var accountClient = MagicOnionClient.Create<IAccountService>(channel);

                 
            }*/

            var client = _grpcClientPool.GetClient();
            try
            {
                // 使用client进行gRPC调用
                authResult = await client.SignInAsync(signInId, pwd);
            }
            finally
            {
                _grpcClientPool.ReleaseClient(client);
            }
            return (authResult!=null && authResult.Success)?  Tuple.Create(true,authResult.Token): Tuple.Create(false,string.Empty);
        }

        [HttpGet(Name = "HttpResponse")]
        
        public List<WeatherForecast> HttpResponse()
        {
            IWeatherForecastApi api = new WeatherForecastApi("https://localhost:5224");
            var res = api.ListWeatherForecastAsync().GetAwaiter().GetResult();
            return res.Data ?? default;
        }

        //[HttpGet(Name = "OrleansResponse")]
        //public async Task<string> OrleansResponse()
        //{
        //    Console.WriteLine(DateTime.Now.ToString());
        //    return await _clusterClient.GetGrain<IFakeMessage>(Random.Shared.Next()).GetMessage();
        //}
    }
}