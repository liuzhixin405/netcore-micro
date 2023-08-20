// https://github.com/alexfoxgill/ExpressionTools

using System;
using System.Linq.Expressions;
using DapperDal.Predicate;

namespace DapperDal.Expressions
{
    /// <summary>
    /// 查询条件表达式转换扩展
    /// </summary>
    public static class PredicateExtensions
    {
        /// <summary>
        /// 查询条件表达式转换为谓词组的扩展方法
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">实体主键类型</typeparam>
        /// <param name="expression">查询条件表达式</param>
        /// <returns>查询条件谓词组</returns>
        public static IPredicate ToPredicateGroup<TEntity, TPrimaryKey>(
            this Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            if (expression == null)
            {
                return null;
            }

            if (ExpressionUtility.IsConstant(expression, true))
            {
                return null;
            }

            if (ExpressionUtility.IsConstant(expression, false))
            {
                return null;
            }

            IPredicate pg = QueryBuilder<TEntity>.FromExpression(expression);

            return pg;
        }
    }
}
