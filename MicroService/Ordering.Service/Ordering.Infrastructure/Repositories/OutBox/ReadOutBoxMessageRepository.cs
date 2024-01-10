using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Dimain.OutBoxMessages;
using Ordering.Domain.Orders;
using Ordering.Infrastructure.Database;
using RepositoryComponent.BaseRepo;
using RepositoryComponent.DbFactories;

namespace Ordering.Infrastructure.Repositories
{
    public class ReadOutBoxMessageRepository: ReadRepository<ReadOrderDbContext, OutBoxMessage>, IReadOutBoxMessageRepository
    {
        private readonly ReadOrderDbContext _readContext;


        public ReadOutBoxMessageRepository(DbFactory<ReadOrderDbContext> readContextFactory) : base(readContextFactory)
        {
            _readContext = readContextFactory?.Context;
        }

        public async ValueTask<OutBoxMessage> GetById(long id)
        {
            var result = await _readContext.Set<OutBoxMessage>().Where(x => x.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<OutBoxMessage>> GetTake(int count)
        {
            var messages = await _readContext.Set<OutBoxMessage>().Where(m => m.ProceddedOnUtc == null)
                      .Take(count).ToListAsync();
            return messages;
        }
    }
}
