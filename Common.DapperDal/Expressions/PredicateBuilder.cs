// https://github.com/alexfoxgill/ExpressionTools

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DapperDal.Utils;

namespace DapperDal.Expressions
{
    /// <summary>
    /// 谓词表达式构建扩展方法
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Produces a predicate expression always returning true
        /// </summary>
        /// <typeparam name="T">The input type for the resulting expression</typeparam>
        /// <returns>A constant expression returning true</returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return _ => true;
        }

        /// <summary>
        /// Produces a predicate expression always returning false
        /// </summary>
        /// <typeparam name="T">The input type for the resulting expression</typeparam>
        /// <returns>A constant expression returning false</returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return _ => false;
        }

        /// <summary>
        /// Combines the first predicate with the second using a logical "and". Short-circuits if either expression is constant
        /// </summary>
        /// <typeparam name="T">The input type for both predicates</typeparam>
        /// <param name="first">The first predicate</param>
        /// <param name="second">The second predicate</param>
        /// <returns>An expression representing a logical "and" between both predicates</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            if (ExpressionUtility.IsConstant(first, true))
                return second;
            if (ExpressionUtility.IsConstant(second, true))
                return first;
            if (ExpressionUtility.IsConstant(first, false) || ExpressionUtility.IsConstant(second, false))
                return False<T>();
            return first.Combine(second, (a, b) => a && b);
        }

        /// <summary>
        /// Combines the first predicate with the second using a logical "or". Short-circuits if either expression is constant
        /// </summary>
        /// <typeparam name="T">The input type for both predicates</typeparam>
        /// <param name="first">The first predicate</param>
        /// <param name="second">The second predicate</param>
        /// <returns>An expression representing a logical "or" between both predicates</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            if (ExpressionUtility.IsConstant(first, false))
                return second;
            if (ExpressionUtility.IsConstant(second, false))
                return first;
            if (ExpressionUtility.IsConstant(first, true) || ExpressionUtility.IsConstant(second, true))
                return True<T>();
            return first.Combine(second, (a, b) => a || b);
        }

        /// <summary>
        /// Composes a negation with the result of the predicate
        /// </summary>
        /// <typeparam name="T">The input type for the predicate</typeparam>
        /// <param name="expr">The predicate</param>
        /// <returns>An expression negating the result of the predicate</returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
        {
            return expr.Compose(x => !x);
        }

        /// <summary>
        /// Performs a reduction on the given list of predicates using logical "and". Returns a <see cref="True{T}"/> expression if the list is empty
        /// </summary>
        /// <typeparam name="T">The input type for all predicates</typeparam>
        /// <param name="predicates">The predicates</param>
        /// <returns>A series of "and" expressions</returns>
        public static Expression<Func<T, bool>> All<T>(this IEnumerable<Expression<Func<T, bool>>> predicates)
        {
            return predicates.Aggregate(True<T>(), And);
        }

        /// <summary>
        /// Performs a reduction on the given list of predicates using logical "and". Returns a <see cref="True{T}"/> expression if the list is empty
        /// </summary>
        /// <typeparam name="T">The input type for all predicates</typeparam>
        /// <param name="predicates">The predicates</param>
        /// <returns>A series of "and" expressions</returns>
        public static Expression<Func<T, bool>> All<T>(params Expression<Func<T, bool>>[] predicates)
        {
            return All(predicates.AsEnumerable());
        }

        /// <summary>
        /// Performs a reduction on the given list of predicates using logical "or". Returns a <see cref="False{T}"/> expression if the list is empty
        /// </summary>
        /// <typeparam name="T">The input type for all predicates</typeparam>
        /// <param name="predicates">The predicates</param>
        /// <returns>A series of "or" expressions</returns>
        public static Expression<Func<T, bool>> Any<T>(this IEnumerable<Expression<Func<T, bool>>> predicates)
        {
            return predicates.Aggregate(False<T>(), Or);
        }

        /// <summary>
        /// Performs a reduction on the given list of predicates using logical "or". Returns a <see cref="False{T}"/> expression if the list is empty
        /// </summary>
        /// <typeparam name="T">The input type for all predicates</typeparam>
        /// <param name="predicates">The predicates</param>
        /// <returns>A series of "or" expressions</returns>
        public static Expression<Func<T, bool>> Any<T>(params Expression<Func<T, bool>>[] predicates)
        {
            return Any(predicates.AsEnumerable());
        }
    }
}