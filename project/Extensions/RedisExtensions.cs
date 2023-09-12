using project.Utility.Helper;

namespace project.Extensions
{
    public static partial class TheExtensions
    {
        public static void AddRedis(this WebApplicationBuilder builder)
        {
            var message = string.Empty;
            Task.WaitAny(new Task[]{
                Task.Run(() => {
               CacheHelper.Init(builder.Configuration); //redis链接不上会死机
             return Task.CompletedTask;
              }), Task.Run(async () => {
             await Task.Delay(1000*30);
                message =$"{nameof(CacheHelper)} 初始化失败,redis可能连接不上,请重试";
             })
            });
            if (!string.IsNullOrEmpty(message)) throw new Exception(message);
        }
    }
}
