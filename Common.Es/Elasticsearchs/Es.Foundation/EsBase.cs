using Nest;
using System.Collections.Generic;

namespace Common.Es
{
    public abstract class EsBase<T>:IEsBase where T : class
    {
        protected readonly EsConfig _esConfig;
        public abstract string IndexName { get; }
        public EsBase(EsConfig esConfig)
        {
            _esConfig = esConfig;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="tList"></param>
        /// <returns></returns>
        public bool InsertMany(List<T> tList)
        {
            var client = _esConfig.GetClient(IndexName);
            if (!client.Indices.Exists(IndexName).Exists)
            {
                client.CreateIndex<T>(IndexName);
            }
            var response = client.IndexMany(tList);
            return response.IsValid;
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <returns></returns>
        public bool CreateIndex()
        {
            var client = _esConfig.GetClient(IndexName);

            if (!client.Indices.Exists(IndexName).Exists)
            {
                var flag = client.CreateIndex<T>(IndexName);
                return flag;
            }

            return false;
        }

        /// <summary>
        /// 创建单个文档
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CreateDocument<TEntity>(TEntity entity, string id) where TEntity : class
        {
            var client = _esConfig.GetClient(IndexName);

            if (!client.Indices.Exists(IndexName).Exists)
            {
                client.CreateIndex<T>(IndexName);
            }

            var response = client.Create<TEntity>(entity, t => t.Index(IndexName).Id(id));
            return response.IsValid;
        }

        /// <summary>
        /// 获取索引文档总数量
        /// </summary>
        /// <returns></returns>
        public long GetTotalCount()
        {
            var client = _esConfig.GetClient(IndexName);
            var search = new SearchDescriptor<T>().MatchAll();
            var response = client.Search<T>(search);
            return response.Total;
        }

        /// <summary>
        /// 根据ID删除文档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(string id)
        {
            var client = _esConfig.GetClient(IndexName);
            var response = client.Delete<T>(id);
            return response.IsValid;
        }

        /// <summary>
        /// 根据ID更新文档
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateDocument<TEntity>(TEntity entity, string id) where TEntity : class
        {
            var client = _esConfig.GetClient(IndexName);
            var response = client.Update<TEntity>(id, t => t.Doc(entity));
            return response.IsValid;
        }
    }
}
