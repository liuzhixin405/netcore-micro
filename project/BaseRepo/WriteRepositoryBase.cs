using EfCoreProject.BaseRepo;
using EfCoreProject.Context;
using System.Linq.Expressions;

namespace project.BaseRepo
{
    public abstract class WriteRepositoryBase<TEntity>:IWriteRepository<TEntity> where TEntity : class
    {
        //public IServiceProvider ServiceProvider { get; }
        //public WriteRepositoryBase(IServiceProvider serviceProvider)
        //=> ServiceProvider = serviceProvider;

        public abstract ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await AddAsync(entity, cancellationToken);
            }
        }

        public abstract Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
      

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity, cancellationToken);
            }
        }

        public abstract Task<TEntity> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
        ;

        public async Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await RemoveAsync(entity, cancellationToken);
            }
        }

        public abstract Task RemoveAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
       ;

        public abstract Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        ;
    }
}
