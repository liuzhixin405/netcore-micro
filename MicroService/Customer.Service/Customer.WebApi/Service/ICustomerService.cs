using Customers.Center.Service.Dtos;
using Customers.Domain.Customers;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace Customers.Center.Service
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomer(LoginDto loginDto);
        Task AddCustomer(AddCustomerDto customerDto);
        Task<TokenDto> GetToken(LoginDto loginDto);
        Task<long> GetUseIdFromToken(string token);
        Task<TokenDto> RefreshToken(string refreshToken);
    }
}
