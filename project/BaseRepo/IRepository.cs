using EfCoreProject.Context;

namespace EfCoreProject.BaseRepository
{
    public interface IWriteRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Add(List<TEntity> entity);
        void Update(List<TEntity> entity);
        void Delete(List<TEntity> entity);
      
        IUnitOfWork GetUnitOfWork();
    }

    public interface IReadRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetQuerable();
        Task<IEnumerable<TEntity>> GetList();
    }
}
