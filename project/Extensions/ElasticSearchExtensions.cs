using Common.Util.Es.Foundation;
using project.Elasticsearchs.Product.Search;

namespace project.Extensions
{
    public static partial class TheExtensions
    {
        public static void AddEsSearch(this WebApplicationBuilder builder)
        {
            var section = builder.Configuration.GetSection("EsConfig");
            builder.Services.AddSingleton(new EsConfig(section.Get<EsOption>()));
            builder.Services.AddTransient<EsProductContext>();
        }
    }
}
