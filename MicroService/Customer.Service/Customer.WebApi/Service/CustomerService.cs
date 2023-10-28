using Customers.Center.Service.Dtos;
using Customers.Domain.Customers;
using Customers.Domain.Seedwork;

namespace Customers.Center.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CustomerService(ICustomerRepository customerRepository,IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddCustomer(AddCustomerDto customerDto)
        {
           await _customerRepository.Add(new Customer { CreateTime = DateTime.Now, PassWord = customerDto.password, UserName = customerDto.user });
           await _unitOfWork.CommitAsync();
        }

        public async Task<Customer> GetCustomer(LoginDto login)
        {
             var customer =await _customerRepository.Get(login.username,login.password);
            return customer==null? MissingCustomer.Instance:customer;
        }
    }
}
