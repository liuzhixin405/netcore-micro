using RepositoryComponent.BaseRepo;
using RepositoryComponent.Page;
using System.Linq.Expressions;

namespace RepositoryComponent.BaseRepo
{
    public abstract class ReadRepositoryBase<TEntity> : IReadRepository<TEntity> where TEntity : class
    {
        //public IServiceProvider ServiceProvider { get; }
        //public ReadRepositoryBase(IServiceProvider serviceProvider)
        //=> ServiceProvider = serviceProvider;

        public abstract Task<TEntity?> FindAsync(IEnumerable<KeyValuePair<string, object>> keyValues, CancellationToken cancellationToken = default)
        ;

        public abstract Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        ;

        public abstract Task<IEnumerable<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
       ;
        public abstract Task<IEnumerable<TEntity>> GetListAsync(string sortField, bool isDescending = true, CancellationToken cancellationToken = default)
        ;

        public abstract Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        ;

        public abstract Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, string sortField, bool isDescending = true, CancellationToken cancellationToken = default)
        ;

        public abstract Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        ;

        public abstract Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
       ;

        public abstract Task<List<TEntity>> GetPaginatedListAsync(int skip, int take, string sortField, bool isDescending = true, CancellationToken cancellationToken = default)
        ;

        public abstract Task<List<TEntity>> GetPaginatedListAsync(int skip, int take, Dictionary<string, bool>? sorting = null, CancellationToken cancellationToken = default)
        ;

        public abstract Task<List<TEntity>> GetPaginatedListAsync(Expression<Func<TEntity, bool>> predicate, int skip, int take, string sortField, bool isDescending = true, CancellationToken cancellationToken = default)
        ;

        public abstract Task<List<TEntity>> GetPaginatedListAsync(Expression<Func<TEntity, bool>> predicate, int skip, int take, Dictionary<string, bool>? sorting = null, CancellationToken cancellationToken = default)
        ;

        public virtual async Task<PaginatedList<TEntity>> GetPaginatedListAsync(PaginatedOptions options, CancellationToken cancellationToken = default)
        {
            var result = await GetPaginatedListAsync((options.Page - 1) * options.PageSize, options.PageSize <= 0 ? int.MaxValue : options.PageSize,
                options.Sorting, cancellationToken);
            var total = await GetCountAsync(cancellationToken);
            return new PaginatedList<TEntity> { Result = result, Total = total, TotalPages = (int)Math.Ceiling(total / (decimal)options.PageSize) };
        }

        public virtual async Task<PaginatedList<TEntity>> GetPaginatedListAsync(Expression<Func<TEntity, bool>> predicate, PaginatedOptions options, CancellationToken cancellationToken = default)
        {
            var result = await GetPaginatedListAsync(predicate,
                (options.Page - 1) * options.PageSize,
                options.PageSize <= 0 ? int.MaxValue : options.PageSize,
                               options.Sorting, cancellationToken);
            var total = await GetCountAsync(predicate, cancellationToken);
            return new PaginatedList<TEntity> { Result = result, Total = total, TotalPages = (int)Math.Ceiling(total / (decimal)options.PageSize) };
        }
    }
}
