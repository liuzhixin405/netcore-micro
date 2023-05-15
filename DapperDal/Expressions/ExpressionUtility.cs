// https://github.com/alexfoxgill/ExpressionTools

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DapperDal.Expressions
{
    /// <summary>
    /// 表达式工具方法
    /// </summary>
    internal static class ExpressionUtility
    {
        /// <summary>
        /// 判断表达式是否是常量表达式
        /// </summary>
        /// <typeparam name="T">表达式参数类型</typeparam>
        /// <typeparam name="TResult">表达式返回类型</typeparam>
        /// <param name="expr">要判断的表达式</param>
        /// <param name="value">常量</param>
        /// <returns>判断结果</returns>
        public static bool IsConstant<T, TResult>(Expression<Func<T, TResult>> expr, TResult value)
        {
            var constant = expr.Body as ConstantExpression;
            if (constant == null)
                return false;
            return constant.Value.Equals(value);
        }

        /// <summary>
        /// 从属性表达式获取属性元数据访问实例
        /// </summary>
        /// <typeparam name="T1">属性表达式参数类型</typeparam>
        /// <typeparam name="T2">属性表达式返回类型</typeparam>
        /// <param name="propertyGetter">属性表达式</param>
        /// <returns>属性元数据访问实例</returns>
        public static PropertyInfo GetPropertyInfo<T1, T2>(this Expression<Func<T1, T2>> propertyGetter)
        {
            var memberExpr = propertyGetter.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException("Expression should be property getter: " + propertyGetter);
            var propInfo = memberExpr.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException("Expression should be property getter: " + propertyGetter);
            return propInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberInit"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        public static MemberInitExpression AddBinding(this MemberInitExpression memberInit, MemberBinding binding)
        {
            var bindings = new List<MemberBinding>(memberInit.Bindings)
            {
                binding
            };
            return Expression.MemberInit(memberInit.NewExpression, bindings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="toReplace"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static Expression Replace(this Expression expression, Expression toReplace, Expression replacement)
        {
            return ExpressionReplacer.Replace(expression, toReplace.Equals, x => replacement);
        }

        public static Expression Replace(this Expression expression, IDictionary<Expression, Expression> map)
        {
            return ExpressionReplacer.Replace(expression, map.ContainsKey, x => map[x]);
        }

        class ExpressionReplacer : ExpressionVisitor
        {
            private readonly Func<Expression, bool> _match;
            private readonly Func<Expression, Expression> _replace;

            private ExpressionReplacer(Func<Expression, bool> match, Func<Expression, Expression> replace)
            {
                _match = match;
                _replace = replace;
            }

            public override Expression Visit(Expression node)
            {
                return _match(node) ? _replace(node) : base.Visit(node);
            }

            public static Expression Replace(Expression expression, Func<Expression, bool> match,
                Func<Expression, Expression> replace)
            {
                return new ExpressionReplacer(match, replace).Visit(expression);
            }
        }
    }
}