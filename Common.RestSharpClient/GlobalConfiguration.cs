using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpComponent
{
    public partial class GlobalConfiguration : Configuration
    {
        private static readonly object GlobalConfigSync = new { };
        private static IReadableConfiguration _globalConfiguration;

        private GlobalConfiguration()
        {

        }
        public GlobalConfiguration(IDictionary<string,string> defaultHeader,string apiV4Key,string apiV4Secret,string basePath = "http://localhost:9001"):base(defaultHeader,apiV4Key,apiV4Secret,basePath)
        {

        }

        static GlobalConfiguration()
        {
            Instance = new GlobalConfiguration();
        }

        public static IReadableConfiguration Instance
        {
            get
            {
                return _globalConfiguration;
            }
            set
            {
                lock(GlobalConfigSync)
                {
                    _globalConfiguration = value;
                }
            }
        }
    }
}
