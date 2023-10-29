using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventureGrainInterfaces;
using Orleans.Concurrency;

namespace AdventureGrains
{
    [Reentrant]
    public class FakeMessage : Grain, IFakeMessage
    {

        public async Task<string> GetMessage()
        {

            for (int i = 1; i < 5; i++)
            {
                await Task.Delay(1000);
                Console.WriteLine($"正在打印第{i}项");
            }


            return ("success");
        }
    }


}
