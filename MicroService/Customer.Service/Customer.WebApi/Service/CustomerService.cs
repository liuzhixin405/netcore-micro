using Customers.Center.Service.Dtos;
using Customers.Domain.Customers;
using Customers.Domain.Seedwork;
using DistributedId;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Caching.Distributed;
using Common.Cache;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Common.Util.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Customers.Center.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedId _distributedId;
        private readonly static string cacheToken = "TokenStr_";
        private IDistributedCache _cache;
        private readonly JwtOptions _jwtOptions;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IDistributedId distributedId, IDistributedCache cache, IOptions<JwtOptions> jwtOptions,JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _distributedId = distributedId;
            _cache = cache;
            _jwtOptions = jwtOptions.Value;
            _tokenHandler = jwtSecurityTokenHandler;
        }

        public async Task AddCustomer(AddCustomerDto customerDto)
        {
            var oldCus = await _customerRepository.Get(customerDto.user, customerDto.password);
            if (oldCus != null) { throw new ArgumentNullException("已经存在该用户"); }
            await _customerRepository.Add(new Customer { Id = _distributedId.NewLongId(), CreateTime = DateTime.Now, PassWord = customerDto.password, UserName = customerDto.user });
            await _unitOfWork.CommitAsync();
        }

        public async Task<Customer> GetCustomer(LoginDto login)
        {
            var customer = await _customerRepository.Get(login.username, login.password);
            return customer == null ? MissingCustomer.Instance : customer;
        }

        public async Task<TokenDto> GetToken(LoginDto dto)
        {
            var result = new TokenDto(false,"", "账号或密码错误");
            
            string getcacheKey = cacheToken + dto.username + "_" + dto.password;

            var user = await GetCustomer(dto);
            if (string.IsNullOrEmpty(user.UserName))
            {
                return result;
            }
             var getFromCache = await _cache.GetObjectAsync<TokenDto>(getcacheKey);
            if (getFromCache == null)
            {

                var claims = new[]
                {
                new Claim("username",user.UserName),
                new Claim("userid",user.Id.ToString())
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
                if (string.IsNullOrWhiteSpace(token))
                {
                    result.Message = "获取token失败";
                }
                result.Token = token;
                result.Message = "";
                result.IsSuccess = true;
                await _cache.SetObjectAsync<TokenDto>(getcacheKey, result, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(5) });
            }
            else
            {
                return getFromCache;
            }
            return result;
        }

        public Task<long> GetUseIdFromToken(String token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
            ClaimsPrincipal principal = null;

            try
            {
                principal = _tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            }
            catch (SecurityTokenException)
            {
                // Token validation failed
                return Task.FromResult(0L);
            }
            var userId = long.Parse(principal.FindFirst("userid")?.Value);
            return Task.FromResult(userId);
        }
    }
}
