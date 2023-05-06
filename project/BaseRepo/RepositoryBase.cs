using EfCoreProject.Context;
using EfCoreProject.DbFactories;
using EfCoreProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace EfCoreProject.BaseRepository
{
    public abstract class RepositoryBase<Ctx,TEntity> : IReadRepository<TEntity> ,IWriteRepository<TEntity> where TEntity : class where Ctx : DbContext
    {
        private readonly Ctx _context;

        public RepositoryBase(DbFactory<Ctx> dbContext)
        {
            _context = dbContext.Context;
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Add(List<TEntity> entity)
        {
            _context.Set<List<TEntity>>().Add(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void Delete(List<TEntity> entity)
        {
            _context.Set<List<TEntity>>().Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetList()
        {
            var result = await _context.Set<TEntity>().AsNoTracking().ToListAsync();
            return result;
        }
        public IQueryable<TEntity> GetQuerable()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<TEntity>().Update(entity);
        }

        public void Update(List<TEntity> entity)
        {
            foreach (var item in entity)
            {
                _context.Entry(item).State = EntityState.Modified;
                _context.Set<TEntity>().Update(item);
            }
        }

        public abstract IUnitOfWork GetUnitOfWork();
    }
}
