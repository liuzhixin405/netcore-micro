using EfCoreProject.Context;

namespace EfCoreProject.BaseRepo
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

   
}
