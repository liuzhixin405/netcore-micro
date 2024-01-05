using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageMiddleware.Kafka
{
    public class BaseOptions
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string BootstrapServers { get; set; }

        /// <summary>
        /// SASL PLAINTEXT认证用户名
        /// </summary>
        public string SaslUsername { get; set; }

        /// <summary>
        /// SASL PLAINTEXT认证密码
        /// </summary>
        public string SaslPassword { get; set; }

    }
}
