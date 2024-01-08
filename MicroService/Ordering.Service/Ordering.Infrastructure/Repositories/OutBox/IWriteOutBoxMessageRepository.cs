using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Dimain.OutBoxMessages;
using Ordering.Domain.Orders;
using RepositoryComponent.BaseRepo;

namespace Ordering.Infrastructure.Repositories
{
    public interface IWriteOutBoxMessageRepository: IWriteRepository<OutBoxMessage>
    {
    }
}
