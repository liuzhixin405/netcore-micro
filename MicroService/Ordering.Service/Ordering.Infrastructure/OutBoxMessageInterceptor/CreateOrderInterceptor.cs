using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Dimain.OutBoxMessages;
using Ordering.Domain.Events;
using Ordering.Infrastructure.Database;

namespace Ordering.Infrastructure.OutBoxMessageInterceptor
{
    public sealed class CreateOrderInterceptor:SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            DbContext? dbContxt = eventData.Context;
            if (dbContxt is null)
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }
            if(eventData.Context.GetType()!=typeof(WriteOrderDbContext))
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }
            var events = dbContxt.ChangeTracker.Entries<IEntity>().Select(x => x.Entity).SelectMany(x =>
            {
                List<IDomainEvent> entities = new List<IDomainEvent>();
                if (x.DomainEvents != null)
                {
                    foreach (var item in x.DomainEvents)
                    {
                        if (!(item is null))
                            entities.Add(item);
                    }
                    x.ClearDomainEvents();
                }
                return entities;
            }).Select(x => new OutBoxMessage
            {
                Id = x.EventId,
                OccurredOnUtc = DateTime.UtcNow,
                Type = x.GetType().Name,
                Content = System.Text.Json.JsonSerializer.Serialize((CreateOrderEvent)x)
            }).ToList();
            if (events != null && events.Any())
                dbContxt.Set<OutBoxMessage>().AddRangeAsync(events);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
