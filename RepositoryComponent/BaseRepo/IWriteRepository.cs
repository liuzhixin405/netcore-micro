using System.Linq.Expressions;

namespace RepositoryComponent.BaseRepo
{
    public interface IWriteRepository<TEntity> where TEntity : class
    {
        //void Add(TEntity entity);
        //void Update(TEntity entity);
        //void Delete(TEntity entity);
        //void Add(List<TEntity> entity);
        //void Update(List<TEntity> entity);
        //void Delete(List<TEntity> entity);

        //IUnitOfWork GetUnitOfWork { get; }
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
        #region Add

        ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        #endregion

        #region Update

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        #endregion

        #region Remove

        Task<TEntity> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task RemoveAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        #endregion
    }
}
