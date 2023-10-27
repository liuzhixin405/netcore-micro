using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Customers.Domain.Customers;
using Customers.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Customers.Infrastructure.Domain.Customers
{
    public class CsutomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _;
        public CsutomerRepository(CustomerContext context)
        {
            _ = context??throw new ArgumentNullException(nameof(context));
        }
        public async Task Add(Customer customer)
        {
            await _.AddAsync(customer);
        }

        public async Task<Customer> Get(string name, string password)
        {
            return await _.Customers.FirstOrDefaultAsync(x=>x.UserName==name&&x.PassWord == password);
        }
    }
}
