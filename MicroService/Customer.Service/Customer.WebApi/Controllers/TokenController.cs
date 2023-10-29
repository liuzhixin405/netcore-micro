using Cache;
using Common.Util.Jwt;
using Customers.Center.Service;
using Customers.Center.Service.Dtos;
using Customers.Domain.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Center.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ICustomerService _customerService;
        private IDistributedCache _cache;
        private readonly static string cacheToken = "TokenStr";
        public TokenController(IOptions<JwtOptions> jwtOptions, ICustomerService customerService, IDistributedCache cache)
        {
            _jwtOptions = jwtOptions.Value;
            _customerService = customerService;
            _cache = cache;
        }
        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse(typeof(TokenDto))]
        public async Task<IActionResult> GetToken([Required] string username, [Required] string password)
        {
            var user = await _customerService.GetCustomer(new Service.Dtos.LoginDto(username, password));
            if (string.IsNullOrEmpty(user.UserName))
            {
                await Task.CompletedTask;
                return Ok(new
                {
                    result = false,
                    token = "",
                    Msg = "账号密码错误"

                });
            }
            var result = await _cache.GetObjectAsync<TokenDto>(cacheToken);
            if (result == null)
            {

                var claims = new[]
                {
                new Claim("username",username)
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

                result = new TokenDto(token, true, "");
                await _cache.SetObjectAsync<TokenDto>(cacheToken, result, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(1) });
            }
            return Ok(result);
        }

        [AllowAnonymous]
        public async Task CreateCustomer(string username, string password)
        {
            await _customerService.AddCustomer(new Service.Dtos.AddCustomerDto(username, password));
        }
    }
}
