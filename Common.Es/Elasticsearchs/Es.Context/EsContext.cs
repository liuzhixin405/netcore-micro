using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Es
{
    public abstract class EsContext<T> : EsBase<T> where T : class
    {
        public EsContext(EsConfig config) : base(config)
        {
        }

        public async Task<EsData<T>> SearchAsync(QueryContainer query, string sortField, bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var client = _esConfig.GetClient(IndexName);

                // 计算分页的起始位置
                var from = (pageNumber - 1) * pageSize;

                var searchResponse = await client.SearchAsync<T>(s => s
          .Index(IndexName)
          .Query(q => query)
          .From(from)  // 设置分页的起始位置
          .Size(pageSize)  // 设置每页大小
          .Sort(sort => ascending
              ? sort.Field(f => f.Field(sortField).Order(SortOrder.Ascending)) // 升序
              : sort.Field(f => f.Field(sortField).Order(SortOrder.Descending)) // 降序
          ).TrackTotalHits(true)
      );

                if (!searchResponse.IsValid)
                {
                    Console.WriteLine(searchResponse.DebugInformation);
                }

                return new EsData<T>
                {
                    List = searchResponse.Documents.ToList(),
                    Total = searchResponse.Total
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EsContext SearchAsync Error: {ex.Message}");
            }
            return new EsData<T>() { List = new List<T>(), Total = 0 };
        }
    }
}
