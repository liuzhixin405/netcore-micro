using Grpc.Net.Client;
using MagicOnion.Client;
using MicroService.Shared;
using MicroService.Shared.GrpcPool;

namespace Login.Client.GrpcClient
{
    public class LoginClientFactory : IGrpcClientFactory<IAccountService>
    {
        public IAccountService Create(GrpcChannel channel)
        {
            return MagicOnionClient.Create<IAccountService>(channel);
        }
    }
}
