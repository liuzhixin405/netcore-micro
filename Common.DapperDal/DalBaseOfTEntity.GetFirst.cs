using System;
using System.Linq;
using System.Linq.Expressions;
using DapperDal.Expressions;

namespace DapperDal
{
    public partial class DalBase<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 获取所有实体列表的第一条
        /// </summary>
        /// <returns>实体列表</returns>
        public async virtual Task<TEntity> GetFirst()
        {
            using (var connection = OpenConnection())
            {
                return (await Configuration.DalImplementor.GetTop<TEntity>(
                        connection: connection,
                        limit: 1,
                        predicate: null,
                        sort: null,
                        transaction: null,
                        commandTimeout: null,
                        buffered: Configuration.Buffered
                    ))?
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据查询条件获取实体列表的第一条
        /// （查询使用谓词或匿名对象）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体列表</returns>
        public async virtual Task<TEntity> GetFirst(object predicate)
        {
            using (var connection = OpenConnection())
            {
                return (await Configuration.DalImplementor.GetTop<TEntity>(
                        connection: connection,
                        limit: 1,
                        predicate: predicate,
                        sort: null,
                        transaction: null,
                        commandTimeout: null,
                        buffered: Configuration.Buffered
                    ))?
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据排序条件获取所有实体列表的第一条
        /// （排序使用表达式）
        /// </summary>
        /// <param name="ascending">排序方向</param>
        /// <param name="sortingExpression">排序字段</param>
        /// <returns>实体列表</returns>
        public async virtual Task<TEntity> GetFirst(SortDirection ascending,
            params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            using (var connection = OpenConnection())
            {
                var sort = sortingExpression.ToSortable(ascending);
                var res = (await Configuration.DalImplementor.GetTop<TEntity>(
                        connection: connection,
                        limit: 1,
                        predicate: null,
                        sort: sort,
                        transaction: null,
                        commandTimeout: null,
                        buffered: Configuration.Buffered
                    ));
                    return res?.FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体列表的第一条
        /// （查询使用谓词或匿名对象，排序使用Sort或匿名对象）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <returns>实体列表</returns>
        public async virtual Task<TEntity> GetFirst(object predicate, object sort)
        {
            using (var connection = OpenConnection())
            {
                var sorts = sort.ToSortable();
                return (await Configuration.DalImplementor.GetTop<TEntity>(
                        connection: connection,
                        limit: 1,
                        predicate: predicate,
                        sort: sorts,
                        transaction: null,
                        commandTimeout: null,
                        buffered: Configuration.Buffered
                    ))?.FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体列表的第一条
        /// （查询使用谓词或匿名对象，排序使用表达式）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="ascending">排序方向</param>
        /// <param name="sortingExpression">排序字段</param>
        /// <returns>实体列表</returns>
        public async virtual Task<TEntity> GetFirst(object predicate,
            SortDirection ascending,
            params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            using (var connection = OpenConnection())
            {
                var sort = sortingExpression.ToSortable(ascending);
                return (await Configuration.DalImplementor.GetTop<TEntity>(
                        connection: connection,
                        limit: 1,
                        predicate: predicate,
                        sort: sort,
                        transaction: null,
                        commandTimeout: null,
                        buffered: Configuration.Buffered
                    ))?
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据查询条件获取实体列表的第一条
        /// （查询使用表达式）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体列表</returns>
        public async virtual Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> predicate)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                return (await Configuration.DalImplementor.GetTop<TEntity>(
                        connection: connection,
                        limit: 1,
                        predicate: predicateGp,
                        sort: null,
                        transaction: null,
                        commandTimeout: null,
                        buffered: Configuration.Buffered
                    ))?.FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体列表的第一条
        /// （查询使用表达式，排序使用Sort或匿名对象）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <returns>实体列表</returns>
        public async virtual Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> predicate, object sort)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                var sorts = sort.ToSortable();
                return (await Configuration.DalImplementor.GetTop<TEntity>(
                        connection: connection,
                        limit: 1,
                        predicate: predicateGp,
                        sort: sorts,
                        transaction: null,
                        commandTimeout: null,
                        buffered: Configuration.Buffered
                    ))?
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体列表的第一条
        /// （查询使用表达式，排序使用表达式）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="ascending">排序方向</param>
        /// <param name="sortingExpression">排序字段</param>
        /// <returns>实体列表</returns>
        public async virtual Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> predicate,
            SortDirection ascending,
            params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                var sorts = sortingExpression.ToSortable(ascending);
                return (await Configuration.DalImplementor.GetTop<TEntity>(
                        connection: connection,
                        limit: 1,
                        predicate: predicateGp,
                        sort: sorts,
                        transaction: null,
                        commandTimeout: null,
                        buffered: Configuration.Buffered
                    ))?.FirstOrDefault();
            }
        }
    }
}
