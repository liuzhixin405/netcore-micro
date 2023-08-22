namespace project.Elasticsearchs.Product.Parameters
{
    public class EsProductParameter
    {
        private const int maxPageSize = 100;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > maxPageSize ? maxPageSize : value;
        }

        public string KeyWords { get; set; }
        public string Content_Prefix { get; set; }
    }
}
