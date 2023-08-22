namespace MessageMiddleware.RabbitMQ
{
    public class RabbitMQSetting
    {
        /// <summary>
        /// 链接地址
        /// </summary>
        public string[] ConnectionString { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        public bool SslEnabled { get; set; }
    }
}
