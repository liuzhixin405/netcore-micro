using AutoMapper;
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
        private readonly IMapper _mapper;
        public CustomerService(IConfiguration configuration,IMapper mapper)
        {
            _connectionString = configuration["ConnectionStrings:SqlServer:WriteConnection"];
            _mapper = mapper;
        }
        public Task Add(CreateCustomerDto cdto)
        {
            new CustomerDal(_connectionString).Insert(
                _mapper.Map<Customer>(cdto));
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Customer>> GetCustomers()
        {
            var result = new CustomerDal(_connectionString).GetList();
            return Task.FromResult(result);
        }
    }
}
