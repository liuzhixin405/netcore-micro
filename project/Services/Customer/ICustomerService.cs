using project.Dtos;
using project.Models;

namespace project.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetCustomers();
        Task Add(CreateCustomerDto cdto);
    }
}
