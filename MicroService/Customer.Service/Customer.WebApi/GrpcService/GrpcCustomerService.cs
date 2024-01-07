using System.Security.Claims;
using System.Text;
using Customers.Center.Service;
using Customers.Domain.Customers;
using GrpcService.CustomerService;
using MagicOnion;
using MagicOnion.Server;
using Microsoft.IdentityModel.Tokens;

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
        public async UnaryResult<UserIdResponse> GetUseIdFromToken(String token)
        {
            var userId = await _customerService.GetUseIdFromToken(token);
            return new UserIdResponse(userId);
        }


    }
}
