using RepositoryComponent.Page;
using System.Linq.Expressions;

namespace RepositoryComponent.BaseRepo
{
    public interface IReadRepository<TEntity> where TEntity : class
    {
        //IQueryable<TEntity> GetQuerable();
        //Task<IEnumerable<TEntity>> GetList();

        #region Find

        Task<TEntity?> FindAsync(IEnumerable<KeyValuePair<string, object>> keyValues, CancellationToken cancellationToken = default);

        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        #endregion

        #region Get

        Task<IEnumerable<TEntity>> GetListAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> GetListAsync(string sortField, bool isDescending = true, CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, string sortField, bool isDescending = true, CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a paginated list after sorting according to skip and take
        /// </summary>
        /// <param name="skip">The number of elements to skip before returning the remaining elements</param>
        /// <param name="take">The number of elements to return</param>
        /// <param name="sortField">Sort field name</param>
        /// <param name="isDescending">true descending order, false ascending order, default: true</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetPaginatedListAsync(int skip, int take, string sortField, bool isDescending = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a paginated list after sorting according to skip and take
        /// </summary>
        /// <param name="skip">The number of elements to skip before returning the remaining elements</param>
        /// <param name="take">The number of elements to return</param>
        /// <param name="sorting">Key: sort field name, Value: true descending order, false ascending order</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetPaginatedListAsync(int skip, int take, Dictionary<string, bool>? sorting = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a paginated list after sorting by condition
        /// </summary>
        /// <param name="predicate"> A function to test each element for a condition</param>
        /// <param name="skip">The number of elements to skip before returning the remaining elements</param>
        /// <param name="take">The number of elements to return</param>
        /// <param name="sortField">Sort field name</param>
        /// <param name="isDescending">true descending order, false ascending order, default: true</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetPaginatedListAsync(Expression<Func<TEntity, bool>> predicate, int skip, int take, string sortField,
            bool isDescending = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a paginated list after sorting by condition
        /// </summary>
        /// <param name="predicate"> A function to test each element for a condition</param>
        /// <param name="skip">The number of elements to skip before returning the remaining elements</param>
        /// <param name="take">The number of elements to return</param>
        /// <param name="sorting">Key: sort field name, Value: true descending order, false ascending order</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetPaginatedListAsync(Expression<Func<TEntity, bool>> predicate, int skip, int take,
            Dictionary<string, bool>? sorting = null, CancellationToken cancellationToken = default);

        Task<PaginatedList<TEntity>> GetPaginatedListAsync(PaginatedOptions options, CancellationToken cancellationToken = default);

        Task<PaginatedList<TEntity>> GetPaginatedListAsync(Expression<Func<TEntity, bool>> predicate, PaginatedOptions options,
            CancellationToken cancellationToken = default);

        #endregion
    }
}
