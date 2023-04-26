using chatgptwriteproject.Context;
using Microsoft.EntityFrameworkCore;

namespace chatgptwriteproject.DbFactories
{
    public class DbFactory<RC,WC>:IDisposable where RC: DbContext where WC: DbContext
    {
        private bool _disposed;
        private Func<Tuple<RC,WC>> _instanceFunc;
        private Tuple<RC, WC> _context;
        public Tuple<RC, WC> Context => _context ?? (_context = _instanceFunc());
        public DbFactory(Func<Tuple<RC, WC>> instance)
        {
            _instanceFunc= instance??throw new ArgumentNullException("dbcontext is null");
        }

        public void Dispose()
        {
            _disposed = true;   
            if(_context.Item1!= null)
            {
                _context.Item1.Dispose();
            }
            if (_context.Item2 != null)
            {
                _context.Item2.Dispose();
            }
        }
    }
}
