using chatgptwriteproject.BaseRepository;
using chatgptwriteproject.Context;
using chatgptwriteproject.Models;

namespace chatgptwriteproject.Repositories
{
    public interface IProductRepository:IRepository<Product>
    {
        ValueTask<Product> GetById(int id);
       
    }
}
