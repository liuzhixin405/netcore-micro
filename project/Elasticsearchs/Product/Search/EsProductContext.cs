using Common.Util.Es.Foundation;
using Nest;
using project.Elasticsearchs.Product.Parameters;
using project.Models;
using  SearchProduct = project.Models.Product;

namespace project.Elasticsearchs.Product.Search
{
    public class EsProductContext:EsBase<SearchProduct>
    {
        public EsProductContext(EsConfig esConfig) : base(esConfig)
        {
        }

        public override string IndexName => "es_product";

        public Task<List<SearchProduct>> Search(EsProductParameter parameter)
        {
            var client = _esConfig.GetClient(IndexName);
            var query = new List<Func<QueryContainerDescriptor<SearchProduct>, QueryContainer>>();
            if (!string.IsNullOrWhiteSpace(parameter.KeyWords))
            {
                query.Add(p => p.Match(m => m.Field(f => f.Name)));
            }

            var search = new SearchDescriptor<SearchProduct>();
            search = search.Query(p => p.Bool(
                b => b.Must(query)))
                .From((parameter.PageNumber - 1) * parameter.PageSize).Size(parameter.PageSize);

            var response = client.Search<SearchProduct>(search);

            return Task.FromResult(response.Documents.ToList());
        }
    }
}
