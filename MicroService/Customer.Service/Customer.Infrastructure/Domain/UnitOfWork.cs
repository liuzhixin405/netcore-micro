using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Customers.Domain.Seedwork;
using Customers.Infrastructure.Database;

namespace Customers.Infrastructure.Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CustomerContext _;
        public UnitOfWork(CustomerContext ordersContext)
        {
                _= ordersContext;
        }
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _.SaveChangesAsync(cancellationToken);
        }
    }
}
