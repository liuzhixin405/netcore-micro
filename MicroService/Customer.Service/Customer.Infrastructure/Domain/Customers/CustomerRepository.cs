using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common.Util;
using Customers.Domain.Customers;
using Customers.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Customers.Infrastructure.Domain.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _;
        public CustomerRepository(CustomerContext context)
        {
            _ = context??throw new ArgumentNullException(nameof(context));
        }
        public async Task Add(Customer customer)
        {
            customer.PassWord = CryptionHelper.CalculateSHA3Hash(customer.PassWord);
            await _.AddAsync(customer);
        }

        public async Task<Customer> Get(string name, string password)
        {
            password = CryptionHelper.CalculateSHA3Hash(password);
            return await _.Customers.FirstOrDefaultAsync(x=>x.UserName==name&&x.PassWord == password);
        }
        public async Task<Customer> GetById(long id)
        {
            return await _.Customers.FirstOrDefaultAsync(x=>x.Id==id);
        }
    }
}
