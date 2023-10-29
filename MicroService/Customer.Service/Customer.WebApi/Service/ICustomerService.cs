using Customers.Center.Service.Dtos;
using Customers.Domain.Customers;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace Customers.Center.Service
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomer(LoginDto login);
        Task AddCustomer(AddCustomerDto customerDto);
    }
}
