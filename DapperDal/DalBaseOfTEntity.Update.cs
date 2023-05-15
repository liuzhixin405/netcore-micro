using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DapperDal.Expressions;
using DapperDal.Predicate;

namespace DapperDal
{
    public partial class DalBase<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 更新指定实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>更新结果</returns>
        public virtual bool Update(TEntity entity)
        {
            using (var connection = OpenConnection())
            {
                return Configuration.DalImplementor.Update(
                    connection: connection,
                    entity: entity,
                    transaction: null,
                    commandTimeout: null);
            }
        }

        /// <summary>
        /// 更新指定实体指定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="props">要更新的属性名列表</param>
        /// <returns>更新结果</returns>
        public virtual bool Update(TEntity entity, IEnumerable<string> props)
        {
            using (var connection = OpenConnection())
            {
                return Configuration.DalImplementor.Update(
                    connection: connection,
                    entity: entity,
                    props: props.ToList(),
                    transaction: null,
                    commandTimeout: null);
            }
        }

        /// <summary>
        /// 更新指定实体指定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="props">要更新的属性名列表，以匿名对象提供</param>
        /// <returns>更新结果</returns>
        public virtual bool Update(TEntity entity, object props)
        {
            using (var connection = OpenConnection())
            {
                return Configuration.DalImplementor.Update(
                    connection: connection,
                    entity: entity,
                    props: props,
                    transaction: null,
                    commandTimeout: null);
            }
        }

        /// <summary>
        /// 更新指定实体指定属性
        /// </summary>
        /// <param name="id">实体主键ID</param>
        /// <param name="props">更新属性名</param>
        /// <returns>更新结果</returns>
        public virtual bool Update(TPrimaryKey id, object props)
        {
            using (var connection = OpenConnection())
            {
                IPredicate predicate = Configuration.DalImplementor.GetIdPredicate<TEntity>(id);

                return Configuration.DalImplementor.Update<TEntity>(
                    connection: connection,
                    props: props,
                    predicate: predicate,
                    transaction: null,
                    commandTimeout: null);
            }
        }

        /// <summary>
        /// 根据指定指定主键ID更新实体指定属性
        /// （条件使用实体主键ID）
        /// </summary>
        /// <param name="keyAndProps">更新实体，包含主键ID、更新属性及值</param>
        /// <returns>更新结果</returns>
        public virtual bool Update(object keyAndProps)
        {
            using (var connection = OpenConnection())
            {
                return Configuration.DalImplementor.Update<TEntity>(
                    connection: connection,
                    keyAndProps: keyAndProps,
                    transaction: null,
                    commandTimeout: null);
            }
        }

        /// <summary>
        /// 根据指定更新条件更新实体指定属性
        /// （条件使用谓词或匿名对象）
        /// </summary>
        /// <param name="props">更新属性及值</param>
        /// <param name="predicate">更新条件，使用谓词或匿名对象</param>
        /// <returns>更新结果</returns>
        public virtual bool Update(object props, object predicate)
        {
            using (var connection = OpenConnection())
            {
                return Configuration.DalImplementor.Update<TEntity>(
                    connection: connection,
                    props: props,
                    predicate: predicate,
                    transaction: null,
                    commandTimeout: null);
            }
        }

        /// <summary>
        /// 根据指定更新条件更新实体指定属性
        /// （条件使用表达式）
        /// </summary>
        /// <param name="props">更新属性及值</param>
        /// <param name="predicate">更新条件，使用表达式</param>
        /// <returns>更新结果</returns>
        public virtual bool Update(object props, Expression<Func<TEntity, bool>> predicate)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();

                return Configuration.DalImplementor.Update<TEntity>(
                    connection: connection,
                    props: props,
                    predicate: predicateGp,
                    transaction: null,
                    commandTimeout: null);
            }
        }
    }
}
