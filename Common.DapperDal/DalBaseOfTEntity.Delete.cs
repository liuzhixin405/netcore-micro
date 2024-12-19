using System;
using System.Linq.Expressions;
using DapperDal.Expressions;
using DapperDal.Predicate;

namespace DapperDal
{
    public partial class DalBase<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>删除结果</returns>
        public virtual Task<bool> Delete(TEntity entity)
        {
            using (var connection = OpenConnection())
            {
                return Configuration.DalImplementor.Delete(
                    connection: connection, 
                    entity: entity, 
                    transaction: null,
                    commandTimeout: null);
            }
        }

        /// <summary>
        /// 根据实体主键ID删除指定实体
        /// </summary>
        /// <param name="id">实体主键ID</param>
        /// <returns>删除结果</returns>
        public virtual Task<bool> Delete(TPrimaryKey id)
        {
            using (var connection = OpenConnection())
            {
                IPredicate predicate = Configuration.DalImplementor.GetIdPredicate<TEntity>(id);

                return Configuration.DalImplementor.Delete<TEntity>(
                    connection: connection,
                    predicate: predicate,
                    transaction: null,
                    commandTimeout: null);
            }
        }

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="predicate">删除条件</param>
        /// <returns>删除结果</returns>
        public virtual Task<bool> Delete(object predicate)
        {
            using (var connection = OpenConnection())
            {
                return Configuration.DalImplementor.Delete<TEntity>(
                    connection: connection,
                    predicate: predicate,
                    transaction: null,
                    commandTimeout: null);
            }
        }

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="predicate">删除条件</param>
        /// <returns>删除结果</returns>
        public virtual Task<bool> Delete(Expression<Func<TEntity, bool>> predicate)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();

                return Configuration.DalImplementor.Delete<TEntity>(
                    connection: connection,
                    predicate: predicateGp,
                    transaction: null,
                    commandTimeout: null);
            }
        }
    }
}
