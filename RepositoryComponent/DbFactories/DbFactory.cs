using Microsoft.EntityFrameworkCore;
using System.Data;

namespace RepositoryComponent.DbFactories
{
    public class DbFactory<Ctx>:IDisposable where Ctx : DbContext
    {
        private bool _disposed;
        private Func<Ctx> _instanceFunc;
        //private DbContextType _dbType;
        private Ctx _context;
        public Ctx Context => _context ?? (_context = _instanceFunc());
        public DbFactory(Func<Ctx> instance
            //, DbContextType dbType
            )
        {
            //_dbType = dbType;
            _instanceFunc = instance??throw new ArgumentNullException("dbcontext is null");
        }

        public void Dispose()
        {
            _disposed = true;   
           
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
