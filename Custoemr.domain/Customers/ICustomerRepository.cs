using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Domain.Customers
{
    public interface ICustomerRepository
    {
        Task<Customer> Get(string name,string password);
        Task Add(Customer customer);
        
    }
}
