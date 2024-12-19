using System;
using System.Linq.Expressions;
using DapperDal.Expressions;
using DapperDal.Predicate;

namespace DapperDal
{
    public partial class DalBase<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 判断实体是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        public virtual async Task<bool> Exsit()
        {
            using (var connection = OpenConnection())
            {
                return (await Configuration.DalImplementor.Count<TEntity>(
                    connection: connection,
                    predicate: null,
                    transaction: null,
                    commandTimeout: null)) > 0;
            }
        }

        /// <summary>
        /// 判断指定主键ID的实体是否存在
        /// </summary>
        /// <param name="id">实体主键ID</param>
        /// <returns>是否存在</returns>
        public async virtual Task<bool> Exsit(TPrimaryKey id)
        {
            using (var connection = OpenConnection())
            {
                IPredicate predicate = Configuration.DalImplementor.GetIdPredicate<TEntity>(id);
                return (await Configuration.DalImplementor.Count<TEntity>(
                           connection: connection,
                           predicate: predicate,
                           transaction: null,
                           commandTimeout: null)) > 0;
            }
        }

        /// <summary>
        /// 判断指定条件的实体是否存在
        /// （条件使用谓词或匿名对象）
        /// </summary>
        /// <param name="predicate">条件，使用谓词或匿名对象</param>
        /// <returns>是否存在</returns>
        public async virtual Task<bool> Exsit(object predicate)
        {
            using (var connection = OpenConnection())
            {
                return ( await Configuration.DalImplementor.Count<TEntity>(
                           connection: connection,
                           predicate: predicate,
                           transaction: null,
                           commandTimeout: null)) > 0;
            }
        }

        /// <summary>
        /// 判断指定条件的实体是否存在
        /// （条件使用表达式）
        /// </summary>
        /// <param name="predicate">条件，使用表达式</param>
        /// <returns>是否存在</returns>
        public async virtual Task<bool> Exsit(Expression<Func<TEntity, bool>> predicate)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                return (await Configuration.DalImplementor.Count<TEntity>(
                    connection: connection,
                    predicate: predicateGp,
                    transaction: null,
                    commandTimeout: null)) > 0;
            }
        }
    }
}
