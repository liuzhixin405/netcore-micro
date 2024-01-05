using RepositoryComponent.DbFactories;
using Microsoft.EntityFrameworkCore;
using RepositoryComponent.Extensions;
using System;
using System.Linq.Expressions;
using System.Threading;

namespace RepositoryComponent.BaseRepo
{
    public class ReadRepository<Ctx, TEntity> : ReadRepositoryBase<TEntity> where TEntity : class where Ctx : DbContext
    {
        private readonly Ctx _context;

        public ReadRepository(DbFactory<Ctx> dbContext)
        {
            _context = dbContext.Context;
        }

        public override Task<TEntity?> FindAsync(IEnumerable<KeyValuePair<string, object>> keyValues, CancellationToken cancellationToken = default)
        {
            Dictionary<string, object> fields = new(keyValues);
            return _context.Set<TEntity>().GetQueryable(fields).FirstOrDefaultAsync(cancellationToken);
        }

        public override Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        =>_context.Set<TEntity>().Where(predicate).FirstOrDefaultAsync(cancellationToken);
        

        public override Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        => _context.Set<TEntity>().LongCountAsync(cancellationToken);

        public override Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        =>_context.Set<TEntity>().LongCountAsync(predicate, cancellationToken);

        public override async Task<IEnumerable<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
       =>await _context.Set<TEntity>().ToListAsync(cancellationToken);

        public override async Task<IEnumerable<TEntity>> GetListAsync(string sortField, bool isDescending = true, CancellationToken cancellationToken = default)
      =>await _context.Set<TEntity>().OrderBy(sortField, isDescending).ToListAsync(cancellationToken);

        public override async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
       =>await _context.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken);

        public override async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, string sortField, bool isDescending = true, CancellationToken cancellationToken = default)
        =>await _context.Set<TEntity>().OrderBy(sortField,isDescending).Where(predicate).ToListAsync(cancellationToken);

        public override async Task<List<TEntity>> GetPaginatedListAsync(int skip, int take, string sortField, bool isDescending = true, CancellationToken cancellationToken = default)
        =>await _context.Set<TEntity>().OrderBy(sortField, isDescending).Skip(skip).Take(take).ToListAsync(cancellationToken);

        public override async Task<List<TEntity>> GetPaginatedListAsync(int skip, int take, Dictionary<string, bool>? sorting = null, CancellationToken cancellationToken = default)
        {
            sorting ??= new Dictionary<string, bool>();
            return await _context.Set<TEntity>().OrderBy(sorting).Skip(skip).Take(take).ToListAsync(cancellationToken);
        }

        public override async Task<List<TEntity>> GetPaginatedListAsync(Expression<Func<TEntity, bool>> predicate, int skip, int take, string sortField, bool isDescending = true, CancellationToken cancellationToken = default)
        =>await _context.Set<TEntity>().Where(predicate).OrderBy(sortField, isDescending).Skip(skip).Take(take).ToListAsync(cancellationToken);

        public override async Task<List<TEntity>> GetPaginatedListAsync(Expression<Func<TEntity, bool>> predicate, int skip, int take, Dictionary<string, bool>? sorting = null, CancellationToken cancellationToken = default)
        {
            sorting ??= new Dictionary<string, bool>();

            return await _context.Set<TEntity>().Where(predicate).OrderBy(sorting).Skip(skip).Take(take).ToListAsync(cancellationToken);
        }
    }
}
