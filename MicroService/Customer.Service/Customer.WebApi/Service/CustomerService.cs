using Customers.Center.Service.Dtos;
using Customers.Domain.Customers;
using Customers.Domain.Seedwork;
using DistributedId;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Customers.Center.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedId _distributedId;
        public CustomerService(ICustomerRepository customerRepository,IUnitOfWork unitOfWork, IDistributedId distributedId)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _distributedId =distributedId;
        }

        public async Task AddCustomer(AddCustomerDto customerDto)
        {
            var oldCus = await _customerRepository.Get(customerDto.user, customerDto.password);
            if(oldCus !=null) { throw new ArgumentNullException("已经存在该用户"); }
           await _customerRepository.Add(new Customer {Id= _distributedId.NewLongId(), CreateTime = DateTime.Now, PassWord = customerDto.password, UserName = customerDto.user });
           await _unitOfWork.CommitAsync();
        }

        public async Task<Customer> GetCustomer(LoginDto login)
        {
             var customer =await _customerRepository.Get(login.username,login.password);
            return customer==null? MissingCustomer.Instance:customer;
        }
    }
}
