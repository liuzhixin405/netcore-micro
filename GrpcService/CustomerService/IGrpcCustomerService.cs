using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagicOnion;
using MessagePack;

namespace GrpcService.CustomerService
{
    public interface IGrpcCustomerService:IService<IGrpcCustomerService>
    {
        UnaryResult<CustomerResponse> GetCustomer(CustomerRequest request);
    }

    [MessagePackObject(true)]
    public class CustomerRequest
    {
       public string UserName { get; set; }
        public string Password { get; set; }

        //public CustomerRequest(string username, string password)
        //{
        //    UserName=username;
        //    Password=password;
        //}
    }
    //public record CustomerRequest(string username, string password)
    //{

    //}
    [MessagePackObject(true)]
    public class CustomerResponse
    {
        public string UserName { get; set; }
        //public CustomerResponse(string username)
        //{
        //    UserName = username;
        //}
    }
    //public record CustomerResponse(string username)
    //{

    //}
}
