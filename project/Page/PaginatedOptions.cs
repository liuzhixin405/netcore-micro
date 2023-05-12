namespace project.Page
{
    public class PaginatedOptions:RequestPageBase
    {
        public Dictionary<String,bool>? Sorting { get; set; }
        public PaginatedOptions()
        {
            
        }

        public PaginatedOptions(int page, int pageSize, Dictionary<string, bool>? sorting=null)
        {
            Page = page;
            PageSize = pageSize;
            Sorting = sorting;
        }

        public PaginatedOptions(int page,int pageSize,string sortField,bool isDescending=true):
            this(page,pageSize,new Dictionary<string, bool>(new List<KeyValuePair<string, bool>> 
            { new(sortField, isDescending) }))
        {
            
        }
    }

    public class PaginatedOptions<T>: PaginatedOptions where T : class
    {
        public T? Search { get; set; }
    }
}
