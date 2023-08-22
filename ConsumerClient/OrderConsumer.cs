using System.Reflection.Metadata;
using MessageMiddleware;

namespace ConsumerClient
{
    public class OrderConsumer : MQListener
    {
        public OrderConsumer(IServiceProvider services) : base(services)
        {
        }

        public override bool Init()
        {
            Exchange = "xx";
            Queue = "xx";
            return true;

        }

        public override async Task<bool> Process(string message)
        {
            Console.WriteLine($"接收到的消息为:{message}");
            await Task.CompletedTask;
            return true;
        }
    }
}
