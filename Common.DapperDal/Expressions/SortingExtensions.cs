using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DapperDal.Predicate;
using DapperDal.Utils;

namespace DapperDal.Expressions
{
    /// <summary>
    /// 排序条件转换扩展
    /// </summary>
    public static class SortingExtensions
    {
        /// <summary>
        /// 排序条件表达式转换为排序组的扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sortingExpression">排序条件表达式</param>
        /// <param name="ascending">排序方向</param>
        /// <returns>排序组</returns>
        public static IList<ISort> ToSortable<T>(this Expression<Func<T, object>>[] sortingExpression,
            SortDirection ascending = SortDirection.Ascending)
        {
            if (sortingExpression == null)
            {
                return null;
            }

            var sortList = new List<ISort>();
            sortingExpression.ToList().ForEach(sortExpression =>
            {
                MemberInfo sortProperty = ReflectionHelper.GetProperty(sortExpression);
                sortList.Add(new Sort
                {
                    Ascending = ascending != SortDirection.Descending,
                    PropertyName = sortProperty.Name
                });
            });

            return sortList;
        }

        /// <summary>
        /// 匿名排序对象转换为排序组的扩展方法
        /// </summary>
        /// <param name="sort">匿名排序对象，如new { CarId = SortDirection.Descending }</param>
        /// <returns>排序组</returns>
        public static IList<ISort> ToSortable(this object sort)
        {
            if (sort == null)
            {
                return null;
            }

            var isortList = sort as IList<ISort>;
            if (isortList != null)
            {
                return isortList;
            }

            var sortList = sort as IList<Sort>;
            if (sortList != null)
            {
                return new List<ISort>(sortList);
            }

            var sorts = new List<ISort>();
            foreach (var kvp in ReflectionHelper.GetObjectValues(sort))
            {
                var ascending = true;

                if (false.Equals(kvp.Value))
                {
                    ascending = false;
                }

                if (SortDirection.Descending.Equals(kvp.Value))
                {
                    ascending = false;
                }

                sorts.Add(new Sort { PropertyName = kvp.Key, Ascending = ascending });
            }

            return sorts;
        }

    }
}
