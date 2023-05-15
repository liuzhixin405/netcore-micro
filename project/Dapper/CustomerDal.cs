using DapperDal;
using project.Models;
using project.Models;

namespace project.Dapper
{
    public class CustomerDal : DalBase<Customer, int>
    {
        public CustomerDal(string configuration) : base(configuration)
        {

        }
    }
}
