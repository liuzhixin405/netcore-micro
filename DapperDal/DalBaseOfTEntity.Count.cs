using System;
using System.Linq.Expressions;
using DapperDal.Expressions;

namespace DapperDal
{
    public partial class DalBase<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 获取实体条数
        /// </summary>
        /// <returns>实体条数</returns>
        public virtual int Count()
        {
            using (var connection = OpenConnection())
            {
                return Configuration.DalImplementor.Count<TEntity>(
                    connection: connection, 
                    predicate: null,
                    transaction: null,
                    commandTimeout: null);
            }
        }

        /// <summary>
        /// 根据条件获取实体条数
        /// （条件使用谓词或匿名对象）
        /// </summary>
        /// <param name="predicate">条件，使用谓词或匿名对象</param>
        /// <returns>实体条数</returns>
        public virtual int Count(object predicate)
        {
            using (var connection = OpenConnection())
            {
                return Configuration.DalImplementor.Count<TEntity>(
                    connection: connection,
                    predicate: predicate,
                    transaction: null,
                    commandTimeout: null);
            }
        }

        /// <summary>
        /// 根据条件获取实体条数
        /// （条件使用表达式）
        /// </summary>
        /// <param name="predicate">条件，使用表达式</param>
        /// <returns>实体条数</returns>
        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                return Configuration.DalImplementor.Count<TEntity>(
                    connection: connection,
                    predicate: predicateGp,
                    transaction: null,
                    commandTimeout: null);
            }
        }
    }
}
