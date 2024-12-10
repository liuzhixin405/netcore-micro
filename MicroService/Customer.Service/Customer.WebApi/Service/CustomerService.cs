using Customers.Center.Service.Dtos;
using Customers.Domain.Customers;
using Customers.Domain.Seedwork;
using Common.DistributedId;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Caching.Distributed;
using Common.Cache;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Common.Util.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Common.Util;

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
        private readonly string CryptionKey = "00000000006666666666660000000000";
        private readonly string CryptionNonce = "999999999999";
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
                    return result;
                }
                result.Token =CryptionHelper.Encrypt(token,CryptionKey,CryptionNonce);
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

        public Task<long> GetUseIdFromToken(String tokenString)
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
            var token = CryptionHelper.Decrypt(tokenString, CryptionKey,CryptionNonce);
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

        // 生成刷新令牌
        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString(); // 简单使用GUID生成刷新令牌
        }

        // 刷新 AccessToken
        public async Task<TokenDto> RefreshToken(string refreshToken)
        {
            var result = new TokenDto(false, "", "无效的刷新令牌");

            // 验证 RefreshToken 是否存在
            var userId = await GetUserIdFromRefreshToken(refreshToken);
            if (userId == 0)
            {
                result.Message = "刷新令牌无效或已过期";
                return result;
            }

            var user = await _customerRepository.GetById(userId);
            if (user == null)
            {
                result.Message = "用户不存在";
                return result;
            }

            // 生成新的 AccessToken
            var claims = new[]
            {
            new Claim("username", user.UserName),
            new Claim("userid", user.Id.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                string.Empty,
                string.Empty,
                claims,
                expires: DateTime.Now.AddHours(_jwtOptions.AccessExpireHours),
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            if (string.IsNullOrWhiteSpace(token))
            {
                result.Message = "获取token失败";
                return result;
            }

            // 生成新的 RefreshToken
            var newRefreshToken = GenerateRefreshToken();

            result.Token = CryptionHelper.Encrypt(token, CryptionKey, CryptionNonce);
            result.RefreshToken = newRefreshToken;
            result.Message = "";
            result.IsSuccess = true;

            // 更新缓存中的刷新令牌
            await _cache.SetStringAsync($"RefreshToken_{user.Id}", newRefreshToken, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(30) });

            return result;
        }

        // 获取用户ID，通过 RefreshToken
        private async Task<long> GetUserIdFromRefreshToken(string refreshToken)
        {
            var userIdStr = await _cache.GetStringAsync($"RefreshToken_{refreshToken}");
            if (string.IsNullOrWhiteSpace(userIdStr))
            {
                return 0;
            }

            return long.Parse(userIdStr);
        }
    }
}
