using Customers.Center.Service;
using GrpcService.CustomerService;
using MagicOnion;
using MagicOnion.Server;

namespace Customers.Center.GrpcService
{
    public class GrpcCustomerService : ServiceBase<IGrpcCustomerService>, IGrpcCustomerService
    {
        private readonly ICustomerService _customerService;
        public GrpcCustomerService(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public async UnaryResult<CustomerResponse> GetCustomer(CustomerRequest request)
        {
            var customer = await _customerService.GetCustomer(new Service.Dtos.LoginDto(request.username, request.password));
            return new CustomerResponse(customer.UserName??"");   
        }
    }
}
