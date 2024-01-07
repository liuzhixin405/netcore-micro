using Common.Cache;
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
       
        private readonly ICustomerService _customerService;
        private readonly ILogger _logger;
        public TokenController(ICustomerService customerService,ILogger<TokenController> logger)
        {
            _logger = logger;
            _customerService = customerService;
        }
        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse(typeof(TokenDto))]
        public async Task<IActionResult> GetToken(LoginDto login)
        {
            var result = new TokenDto(false, "", "error");

            if (string.IsNullOrWhiteSpace(login.username) || string.IsNullOrWhiteSpace(login.password))
            {
                return Ok(result);
            }
            try
            {
                return Ok(await _customerService.GetToken(login));
            }catch(Exception ex)
            {
                _logger.LogError($"获取token报错,错误消息: {ex.Message}");
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
