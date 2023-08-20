using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.Dapper;
using project.Dtos;
using project.Models;
using project.Services;
using project.Utility.Helper;

namespace project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        [Route("AddCustomer")]
        public Task AddCustomer(CreateCustomerDto dto)
        {
            _customerService.Add(dto);
            return Task.CompletedTask;
        }
        [HttpGet]
        [Route("GetCustomerList")]

        public async Task<IEnumerable<Customer>> GetCustomerList()
        {
           var result =await _customerService.GetCustomers();
            return result;
        }
    }
}
