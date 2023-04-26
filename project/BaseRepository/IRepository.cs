using chatgptwriteproject.Context;

namespace chatgptwriteproject.BaseRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetList();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Add(List<TEntity> entity);
        void Update(List<TEntity> entity);
        void Delete(List<TEntity> entity);
        IQueryable<TEntity> GetQuerable();
        IUnitOfWork GetUnitOfWork();
    }
}
