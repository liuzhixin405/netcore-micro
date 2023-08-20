using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DapperDal.Expressions;

namespace DapperDal
{
    public partial class DalBase<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 根据查询条件和排序条件获取实体分页列表
        /// （查询使用谓词或匿名对象，排序使用Sort或匿名对象）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <param name="pageNumber">页号，从1起始</param>
        /// <param name="itemsPerPage">每页条数</param>
        /// <returns>实体分页列表</returns>
        public virtual IEnumerable<TEntity> GetListPaged(object predicate, object sort,
            int pageNumber, int itemsPerPage)
        {
            using (var connection = OpenConnection())
            {
                var sorts = sort.ToSortable();
                return Configuration.DalImplementor.GetPage<TEntity>(
                    connection: connection,
                    predicate: predicate,
                    sort: sorts,
                    page: pageNumber - 1,
                    resultsPerPage: itemsPerPage,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体分页列表
        /// （查询使用谓词或匿名对象，排序表达式）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="pageNumber">页号，从1起始</param>
        /// <param name="itemsPerPage">每页条数</param>
        /// <param name="ascending">排序方向</param>
        /// <param name="sortingExpression">排序字段</param>
        /// <returns>实体分页列表</returns>
        public virtual IEnumerable<TEntity> GetListPaged(object predicate,
            int pageNumber, int itemsPerPage,
            SortDirection ascending,
            params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            using (var connection = OpenConnection())
            {
                var sort = sortingExpression.ToSortable(ascending);
                return Configuration.DalImplementor.GetPage<TEntity>(
                    connection: connection,
                    predicate: predicate,
                    sort: sort,
                    page: pageNumber - 1,
                    resultsPerPage: itemsPerPage,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体分页列表
        /// （查询使用表达式，排序使用Sort或匿名对象）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <param name="pageNumber">页号，从1起始</param>
        /// <param name="itemsPerPage">每页条数</param>
        /// <returns>实体分页列表</returns>
        public virtual IEnumerable<TEntity> GetListPaged(Expression<Func<TEntity, bool>> predicate, object sort,
            int pageNumber, int itemsPerPage)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                var sorts = sort.ToSortable();
                return Configuration.DalImplementor.GetPage<TEntity>(
                    connection: connection,
                    predicate: predicateGp,
                    sort: sorts,
                    page: pageNumber - 1,
                    resultsPerPage: itemsPerPage,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }

        /// <summary>
        /// 根据查询条件和排序条件获取实体分页列表
        /// （查询使用表达式，排序使用表达式）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="pageNumber">页号，从1起始</param>
        /// <param name="itemsPerPage">每页条数</param>
        /// <param name="ascending">排序方向</param>
        /// <param name="sortingExpression">排序字段</param>
        /// <returns>实体分页列表</returns>
        public virtual IEnumerable<TEntity> GetListPaged(Expression<Func<TEntity, bool>> predicate,
            int pageNumber, int itemsPerPage,
            SortDirection ascending,
            params Expression<Func<TEntity, object>>[] sortingExpression)
        {
            using (var connection = OpenConnection())
            {
                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                var sort = sortingExpression.ToSortable(ascending);
                return Configuration.DalImplementor.GetPage<TEntity>(
                    connection: connection,
                    predicate: predicateGp,
                    sort: sort,
                    page: pageNumber - 1,
                    resultsPerPage: itemsPerPage,
                    transaction: null,
                    commandTimeout: null,
                    buffered: Configuration.Buffered);
            }
        }
    }
}
