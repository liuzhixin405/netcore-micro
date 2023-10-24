using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpComponent
{
    public class RequestOptions
    {
        public Dictionary<string,string> PathParameters { get; set; }

        public Multimap<string,string> QueryParameters { get; set; }
        public Multimap<string,string> HeaderParameters { get; set; }
        public Dictionary<string,string> FormParameters { get; set; }
        public Dictionary<string,Stream> FileParameters { get; set; }
        public List<Cookie> Cookies { get; set; }
        public object Data { get; set; }
        public bool RequireApiV4Auth { get; set; }
        public RequestOptions()
        {
            PathParameters = new Dictionary<string, string>();
            QueryParameters = new Multimap<string, string>();
            HeaderParameters = new Multimap<string, string>();
            FormParameters = new Dictionary<string, string>();
            FileParameters = new Dictionary<String, Stream>();
            Cookies = new List<Cookie>();
            RequireApiV4Auth = false;
        }
    }
}
