using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;

namespace MicroService.Shared.GrpcPool
{
    public interface IGrpcClientFactory<Tclient>
    {
        Tclient Create(GrpcChannel channel);
    }
}
