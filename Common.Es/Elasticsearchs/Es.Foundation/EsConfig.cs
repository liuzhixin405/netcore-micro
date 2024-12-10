using Nest;
using System;
using System.Linq;

namespace Common.Es
{
    public class EsConfig
    {
        private readonly EsOption _option;
        public EsConfig(EsOption option)
        {
            _option = option;
        }

        public ElasticClient GetClient(string indexName = "")
        {
            if(_option.Urls==null || _option.Urls.Count() == 0)
            {
                throw new Exception("es 地址不可为空");
            }
            //var uris = urls.Select(p => new Uri(p)).ToArray();
            //var connectionPool = new SniffingConnectionPool(uris);
            //var connectionSetting = new ConnectionSettings(connectionPool); //单机状态使用集群导致链接docker 的ip导致链接失败报错(该es虚拟机下doker单机部署的)
            var uri = new Uri(_option.Urls[0]);
            var connectionSettings = new ConnectionSettings(uri).BasicAuthentication(_option.Username, _option.Password);
            if (!string.IsNullOrWhiteSpace(_option.Urls[0]))
            {
                connectionSettings.DefaultIndex(indexName);
            }
            return new ElasticClient(connectionSettings);
        }
    }
}
