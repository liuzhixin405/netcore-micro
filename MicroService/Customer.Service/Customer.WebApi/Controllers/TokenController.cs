using Common.Util.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Center.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly JwtOptions _jwtOptions;
        public TokenController(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        [AllowAnonymous]
        [HttpGet]
        public string GetToken()
        {
            var userId = "admin";       //账号密码处理略

            var claims = new[]
            {
                new Claim("userId",userId)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                string.Empty,
                string.Empty,
                claims,
                expires: DateTime.Now.AddHours(_jwtOptions.AccessExpireHours),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return JsonConvert.SerializeObject(new
            {
                result = true,
                token

            });
        }
    }
}
