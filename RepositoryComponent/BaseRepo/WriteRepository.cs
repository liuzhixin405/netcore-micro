
using RepositoryComponent.DbFactories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RepositoryComponent.BaseRepo;
using System.Linq.Expressions;

namespace RepositoryComponent.BaseRepo
{
    public class WriteRepository<Ctx,TEntity> : WriteRepositoryBase<TEntity> where TEntity : class where Ctx : DbContext
    {
        private readonly Ctx _context;

        public WriteRepository(DbFactory<Ctx> dbContext)
        {
            _context = dbContext.Context;
        }

        

        public override async ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
             await _context.AddAsync(entity, cancellationToken);
            return entity;
        }

        public override Task<TEntity> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
             _context.Set<TEntity>().Remove(entity);
            return Task.FromResult(entity);
        }

        public override Task RemoveAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        public override Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
           return _context.SaveChangesAsync(cancellationToken);
        }

        public override Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _context.Set<TEntity>().Update(entity);
            
            return Task.FromResult(entity);
        }
    }
}
