namespace Ordering.WebApi
{

    public static class Const
    {
        public const string Normal_Queue = "paying_exchange.normal_queue";
        public const string Normal_Exchange = "paying_exchange.normal";
        public const string Normal_RoutingKey = "paying_exchange.normal_key";
        public const string Delay_Queue = "paying_exchange.dl_queue";
        public const string Delay_Exchange = "paying_exchange.dl";
        public const string Delay_RoutingKey = "paying_exchange.dl_key";
        public const long DelayTime = 1000*60*5;
    }
    public record RabbitMqConfig
    {
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "admin";
        public string Password { get; set; } = "admin";
    }
}

