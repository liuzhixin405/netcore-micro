using project.Dapper;
using project.Dtos;
using project.Models;
using project.Utility.Helper;
using System.Configuration;

namespace project.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly string _connectionString;
        public CustomerService(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:SqlServer:WriteConnection"];
        }
        public Task Add(CreateCustomerDto cdto)
        {
            new CustomerDal(_connectionString).Insert(new
               Customer()
            {
                FirstName = cdto.FirstName,
                LastName = cdto.LastName,
                CreateTime = TimestampHelper.ToUnixTimeMilliseconds(DateTime.UtcNow)
            });
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Customer>> GetCustomers()
        {
            var result = new CustomerDal(_connectionString).GetList();
            return Task.FromResult(result);
        }
    }
}
