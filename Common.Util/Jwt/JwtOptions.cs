using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util.Jwt
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public int AccessExpireHours { get; set; }
        public int RefreshExpireHours { get; set; }
    }
}
