using Newtonsoft.Json;

namespace MessageMiddleware
{
    public interface IMQPublisher
    {
        /// <summary>
        /// 发送队列消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routingKey"></param>
        /// <param name="settings">
        /// 序列化方式，
        /// 兼容 T 有子类，反序列化问题
        /// </param>
        /// <returns></returns>
        bool Publish<T>(T obj, string exchange, string queue = "", string routingKey = "#", JsonSerializerSettings settings = null);
    }
}
