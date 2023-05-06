namespace EfCoreProject.BaseRepo
{
    public interface IReadRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetQuerable();
        Task<IEnumerable<TEntity>> GetList();
    }
}
