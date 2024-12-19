using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DapperDal.Expressions;

namespace DapperDal
{
    public partial class DalBase<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 获取所有实体列表
        /// </summary>
        /// <returns>实体列表</returns>
        public virtual Task<IEnumerable<TEntity>> GetList()
        {
            using (var connection = OpenConnection())
            {
                return Configuration.DalImplementor.GetList<TEntity>(
                    connection: connection,
                    predicate: null,
                    sort: null,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }

        /// <summary>
        /// 根据查询条件获取实体列表
        /// （查询使用谓词或匿名对象）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体列表</returns>
        public virtual Task<IEnumerable<TEntity>> GetList(object predicate)
        {
            using (var connection = OpenConnection())
            {
                return Configuration.DalImplementor.GetList<TEntity>(
                    connection: connection,
                    predicate: predicate,
                    sort: null,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }

        /// <summary>
        /// 根据排序条件获取所有实体列表
        /// （排序使用表达式）
        /// </summary>
        /// <param name="ascending">排序方向</param>
        /// <param name="sortingExpression">排序字段</param>
        /// <returns>实体列表</returns>
        public virtual Task<IEnumerable<TEntity>> GetList(SortDirection ascending,
            params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            using (var connection = OpenConnection())
            {
                var sort = sortingExpression.ToSortable(ascending);
                return Configuration.DalImplementor.GetList<TEntity>(
                    connection: connection,
                    predicate: null,
                    sort: sort,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体列表
        /// （查询使用谓词或匿名对象，排序使用Sort或匿名对象）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <returns>实体列表</returns>
        public virtual Task<IEnumerable<TEntity>> GetList(object predicate, object sort)
        {
            using (var connection = OpenConnection())
            {
                var sorts = sort.ToSortable();
                return Configuration.DalImplementor.GetList<TEntity>(
                    connection: connection,
                    predicate: predicate,
                    sort: sorts,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体列表
        /// （查询使用谓词或匿名对象，排序使用表达式）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="ascending">排序方向</param>
        /// <param name="sortingExpression">排序字段</param>
        /// <returns>实体列表</returns>
        public virtual Task<IEnumerable<TEntity>> GetList(object predicate,
            SortDirection ascending,
            params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            using (var connection = OpenConnection())
            {
                var sort = sortingExpression.ToSortable(ascending);
                return Configuration.DalImplementor.GetList<TEntity>(
                    connection: connection,
                    predicate: predicate,
                    sort: sort,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }

        /// <summary>
        /// 根据查询条件获取实体列表
        /// （查询使用表达式）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体列表</returns>
        public virtual Task<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                return Configuration.DalImplementor.GetList<TEntity>(
                    connection: connection,
                    predicate: predicateGp,
                    sort: null,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体列表
        /// （查询使用表达式，排序使用Sort或匿名对象）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <returns>实体列表</returns>
        public virtual Task<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, object sort)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                var sorts = sort.ToSortable();
                return Configuration.DalImplementor.GetList<TEntity>(
                    connection: connection,
                    predicate: predicateGp,
                    sort: sorts,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体列表
        /// （查询使用表达式，排序使用表达式）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="ascending">排序方向</param>
        /// <param name="sortingExpression">排序字段</param>
        /// <returns>实体列表</returns>
        public virtual Task<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate,
            SortDirection ascending,
            params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                var sort = sortingExpression.ToSortable(ascending);
                return Configuration.DalImplementor.GetList<TEntity>(
                    connection: connection,
                    predicate: predicateGp,
                    sort: sort,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }
    }
}
