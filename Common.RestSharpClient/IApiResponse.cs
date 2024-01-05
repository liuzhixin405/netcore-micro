using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpComponent
{
    /// <summary>
    /// Provides a non-generic contract for the ApiResponse wrapper.
    /// </summary>
    public interface IApiResponse
    {
        Type ResponseType { get; }
        Object Content { get; }
        HttpStatusCode StatusCode { get; }
        Multimap<string, string> Headers { get; }
        string ErrorText { get; set; }
        List<Cookie> Cookies { get; set; }
        string RawContent { get; }
    }

    /// <summary>
    /// IApi Response Implement
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T> : IApiResponse
    {
        public ApiResponse(HttpStatusCode statusCode, Multimap<string, string> headers, T data, string rawContent)
        {
            StatusCode = statusCode;
            Headers = headers;
            Data = data;
        }
        public ApiResponse(HttpStatusCode statusCode, Multimap<string, string> headers, T data) : this(statusCode, headers, data, null) { }
        public ApiResponse(HttpStatusCode statusCode, T data, string rawContent) : this(statusCode, null, data, rawContent)
        {

        }
        public ApiResponse(HttpStatusCode statusCode, T data) : this(statusCode, data, null)
        {

        }
        public T Data { get; }
        public Type ResponseType => typeof(T);

        public object Content => Data;

        public HttpStatusCode StatusCode { get; }

        public Multimap<string, string> Headers { get; }

        public string ErrorText { get; set; }
        public List<Cookie> Cookies { get; set; }

        public string RawContent { get; }
    }
}
