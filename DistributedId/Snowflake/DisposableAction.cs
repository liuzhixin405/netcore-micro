using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedId
{
    public class DisposableAction:IDisposable
    {
        readonly Action _action;
        public DisposableAction(Action action)=>_action = action?? throw new ArgumentNullException("action");
        
        public void Dispose() => _action();
    }
}
