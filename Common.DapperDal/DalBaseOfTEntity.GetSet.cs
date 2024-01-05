using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DapperDal.Expressions;

namespace DapperDal
{
    public partial class DalBase<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 根据查询条件和排序条件获取实体区间列表
        /// （查询使用谓词或匿名对象，排序使用Sort或匿名对象）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <param name="firstResult">起始行数</param>
        /// <param name="maxResults">最大条数</param>
        /// <returns>实体区间列表</returns>
        public virtual IEnumerable<TEntity> GetSet(object predicate, object sort,
            int firstResult, int maxResults)
        {
            using (var connection = OpenConnection())
            {
                var sorts = sort.ToSortable();
                return Configuration.DalImplementor.GetSet<TEntity>(
                    connection: connection,
                    predicate: predicate,
                    sort: sorts,
                    firstResult: firstResult,
                    maxResults: maxResults,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体区间列表
        /// （查询使用谓词或匿名对象，排序表达式）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="firstResult">起始行数</param>
        /// <param name="maxResults">最大条数</param>
        /// <param name="ascending">排序方向</param>
        /// <param name="sortingExpression">排序字段</param>
        /// <returns>实体区间列表</returns>
        public virtual IEnumerable<TEntity> GetSet(object predicate,
            int firstResult, int maxResults,
            SortDirection ascending,
            params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            using (var connection = OpenConnection())
            {
                var sort = sortingExpression.ToSortable(ascending);
                return Configuration.DalImplementor.GetSet<TEntity>(
                    connection: connection,
                    predicate: predicate,
                    sort: sort,
                    firstResult: firstResult,
                    maxResults: maxResults,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体区间列表
        /// （查询使用表达式，排序使用Sort或匿名对象）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <param name="firstResult">起始行数</param>
        /// <param name="maxResults">最大条数</param>
        /// <returns>实体区间列表</returns>
        public virtual IEnumerable<TEntity> GetSet(Expression<Func<TEntity, bool>> predicate, object sort,
            int firstResult, int maxResults)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                var sorts = sort.ToSortable();
                return Configuration.DalImplementor.GetSet<TEntity>(
                    connection: connection,
                    predicate: predicateGp,
                    sort: sorts,
                    firstResult: firstResult,
                    maxResults: maxResults,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体区间列表
        /// （查询使用表达式，排序使用表达式）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="firstResult">起始行数</param>
        /// <param name="maxResults">最大条数</param>
        /// <param name="ascending">排序方向</param>
        /// <param name="sortingExpression">排序字段</param>
        /// <returns>实体区间列表</returns>
        public virtual IEnumerable<TEntity> GetSet(Expression<Func<TEntity, bool>> predicate,
            int firstResult, int maxResults,
            SortDirection ascending,
            params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                var sort = sortingExpression.ToSortable(ascending);
                return Configuration.DalImplementor.GetSet<TEntity>(
                    connection: connection,
                    predicate: predicateGp,
                    sort: sort,
                    firstResult: firstResult,
                    maxResults: maxResults,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }
    }
}
