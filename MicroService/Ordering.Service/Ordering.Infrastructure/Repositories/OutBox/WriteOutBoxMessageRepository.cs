using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Dimain.OutBoxMessages;
using Ordering.Domain.Orders;
using Ordering.Infrastructure.Database;
using RepositoryComponent.BaseRepo;
using RepositoryComponent.DbFactories;

namespace Ordering.Infrastructure.Repositories
{
    internal class WriteOutBoxMessageRepository : WriteRepository<WriteOrderDbContext, OutBoxMessage>, IWriteOutBoxMessageRepository
    {
        private readonly WriteOrderDbContext _writeContext;


        public WriteOutBoxMessageRepository(DbFactory<WriteOrderDbContext> writeContextFactory) : base(writeContextFactory)
        {
            _writeContext = writeContextFactory?.Context;
        }
    }
}
