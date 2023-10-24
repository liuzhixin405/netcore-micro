using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGrainInterfaces
{
    public interface IFakeMessage: IGrainWithIntegerKey
    {
        Task<string>  GetMessage();
    }

    public interface IFakeMessageNext: IGrainWithGuidKey
    {
        Task<string> GetMessage();
    }
}
