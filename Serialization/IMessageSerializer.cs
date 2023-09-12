using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    public interface IMessageSerializer
    {
        byte[] Serialize(Type t, object message);
    }

    public interface IMessageDeserializer
    {
        object Deserialize(Type t, byte[] payload);
    }
}
