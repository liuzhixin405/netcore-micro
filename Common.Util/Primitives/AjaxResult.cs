namespace Common.Util.Primitives
{
    public class AjaxResult
    {
        public bool Success { get; set; } = true;
        public int ErrorCOde { get; set; }
        public string Msg { get; set; }
    }

    public class AjaxResult<T> : AjaxResult
    {
        public T Data { get; set; }
    }
}