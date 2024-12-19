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
        public async Task Add(CreateCustomerDto cdto)
        {
           await new CustomerDal(_connectionString).Insert(
                _mapper.Map<Customer>(cdto));
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            var result =await new CustomerDal(_connectionString).GetList();
            return result;
        }
    }
}
