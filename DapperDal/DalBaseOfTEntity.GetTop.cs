using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DapperDal.Expressions;

namespace DapperDal
{
    public partial class DalBase<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 获取所有实体列表的前N条
        /// </summary>
        /// <param name="limit">前几条</param>
        /// <returns>实体列表</returns>
        public virtual IEnumerable<TEntity> GetTop(int limit)
        {
            using (var connection = OpenConnection())
            {
                return Configuration.DalImplementor.GetTop<TEntity>(
                    connection: connection,
                    limit: limit,
                    predicate: null,
                    sort: null,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered
                );
            }
        }

        /// <summary>
        /// 根据查询条件获取实体列表的前N条
        /// （查询使用谓词或匿名对象）
        /// </summary>
        /// <param name="limit">前几条</param>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体列表</returns>
        public virtual IEnumerable<TEntity> GetTop(int limit, object predicate)
        {
            using (var connection = OpenConnection())
            {
                return Configuration.DalImplementor.GetTop<TEntity>(
                    connection: connection,
                    limit: limit,
                    predicate: predicate,
                    sort: null,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered
                );
            }
        }

        /// <summary>
        /// 根据排序条件获取所有实体列表的前N条
        /// （排序使用表达式）
        /// </summary>
        /// <param name="limit">前几条</param>
        /// <param name="ascending">排序方向</param>
        /// <param name="sortingExpression">排序字段</param>
        /// <returns>实体列表</returns>
        public virtual IEnumerable<TEntity> GetTop(int limit, SortDirection ascending,
            params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            using (var connection = OpenConnection())
            {
                var sort = sortingExpression.ToSortable(ascending);
                return Configuration.DalImplementor.GetTop<TEntity>(
                    connection: connection,
                    limit: limit,
                    predicate: null,
                    sort: sort,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered
                );
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体列表的前N条
        /// （查询使用谓词或匿名对象，排序使用Sort或匿名对象）
        /// </summary>
        /// <param name="limit">前几条</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <returns>实体列表</returns>
        public virtual IEnumerable<TEntity> GetTop(int limit, object predicate, object sort)
        {
            using (var connection = OpenConnection())
            {
                var sorts = sort.ToSortable();
                return Configuration.DalImplementor.GetTop<TEntity>(
                        connection: connection,
                        limit: limit,
                        predicate: predicate,
                        sort: sorts,
                        transaction: null,
                        commandTimeout: null,
                        buffered: Configuration.Buffered
                    );
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体列表的前N条
        /// （查询使用谓词或匿名对象，排序使用表达式）
        /// </summary>
        /// <param name="limit">前几条</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="ascending">排序方向</param>
        /// <param name="sortingExpression">排序字段</param>
        /// <returns>实体列表</returns>
        public virtual IEnumerable<TEntity> GetTop(int limit, object predicate,
            SortDirection ascending,
            params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            using (var connection = OpenConnection())
            {
                var sort = sortingExpression.ToSortable(ascending);
                return Configuration.DalImplementor.GetTop<TEntity>(
                        connection: connection,
                        limit: limit,
                        predicate: predicate,
                        sort: sort,
                        transaction: null,
                        commandTimeout: null,
                        buffered: Configuration.Buffered
                    );
            }
        }

        /// <summary>
        /// 根据查询条件获取实体列表的前N条
        /// （查询使用表达式）
        /// </summary>
        /// <param name="limit">前几条</param>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体列表</returns>
        public virtual IEnumerable<TEntity> GetTop(int limit, Expression<Func<TEntity, bool>> predicate)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                return Configuration.DalImplementor.GetTop<TEntity>(
                        connection: connection,
                        limit: limit,
                        predicate: predicateGp,
                        sort: null,
                        transaction: null,
                        commandTimeout: null,
                        buffered: Configuration.Buffered
                    );
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体列表的前N条
        /// （查询使用表达式，排序使用Sort或匿名对象）
        /// </summary>
        /// <param name="limit">前几条</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <returns>实体列表</returns>
        public virtual IEnumerable<TEntity> GetTop(int limit, Expression<Func<TEntity, bool>> predicate, object sort)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                var sorts = sort.ToSortable();
                return Configuration.DalImplementor.GetTop<TEntity>(
                        connection: connection,
                        limit: limit,
                        predicate: predicateGp,
                        sort: sorts,
                        transaction: null,
                        commandTimeout: null,
                        buffered: Configuration.Buffered
                    );
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体列表的前N条
        /// （查询使用表达式，排序使用表达式）
        /// </summary>
        /// <param name="limit">前几条</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="ascending">排序方向</param>
        /// <param name="sortingExpression">排序字段</param>
        /// <returns>实体列表</returns>
        public virtual IEnumerable<TEntity> GetTop(int limit, Expression<Func<TEntity, bool>> predicate,
            SortDirection ascending,
            params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                var sorts = sortingExpression.ToSortable(ascending);
                return Configuration.DalImplementor.GetTop<TEntity>(
                        connection: connection,
                        limit: limit,
                        predicate: predicateGp,
                        sort: sorts,
                        transaction: null,
                        commandTimeout: null,
                        buffered: Configuration.Buffered
                    );
            }
        }
    }
}
