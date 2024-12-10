using Nest;

namespace Common.Es
{
    /// <summary>
    /// es扩展类
    /// </summary>
    public static class EsClientExtension
    {

        public static bool CreateIndex<T>(this ElasticClient elasticClient,string indexName="",int numberOfShards=5,int numberOfReplicas=1) where T : class
        {
            if (string.IsNullOrWhiteSpace(indexName))
            {
                indexName = typeof(T).Name;
            }
            if(elasticClient.Indices.Exists(indexName).Exists) { return false; }
            else
            {
                var indexState = new IndexState();
                IndexSettings settings = new IndexSettings()
                {
                    NumberOfReplicas = numberOfReplicas,
                    NumberOfShards = numberOfShards,
                };
                var response = elasticClient.Indices.Create(indexName, p => p.InitializeUsing(indexState).Map<T>(p => p.AutoMap()));
                return response.Acknowledged;
            }
        }
    }
}
