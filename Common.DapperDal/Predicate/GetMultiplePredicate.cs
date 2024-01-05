using System;
using System.Collections.Generic;

namespace DapperDal.Predicate
{
    /// <summary>
    /// 表示多结果集的谓词组
    /// </summary>
    public class GetMultiplePredicate
    {
        private readonly List<GetMultiplePredicateItem> _items;

        /// <summary>
        /// 初始化多结果集的谓词组
        /// </summary>
        public GetMultiplePredicate()
        {
            _items = new List<GetMultiplePredicateItem>();
        }

        /// <summary>
        /// 谓词组集合
        /// </summary>
        public IEnumerable<GetMultiplePredicateItem> Items
        {
            get { return _items.AsReadOnly(); }
        }

        /// <summary>
        /// 添加一条结果集谓词组
        /// </summary>
        /// <param name="predicate">谓词</param>
        /// <param name="sort">排序条件</param>
        /// <typeparam name="T">结果集实体类型</typeparam>
        public void Add<T>(IPredicate predicate, IList<ISort> sort = null) where T : class
        {
            _items.Add(new GetMultiplePredicateItem
                           {
                               Value = predicate,
                               Type = typeof(T),
                               Sort = sort
                           });
        }

        /// <summary>
        /// 添加一条结果集谓词组
        /// </summary>
        /// <param name="id">谓词</param>
        /// <typeparam name="T">结果集实体类型</typeparam>
        public void Add<T>(object id) where T : class
        {
            _items.Add(new GetMultiplePredicateItem
                           {
                               Value = id,
                               Type = typeof (T)
                           });
        }

        /// <summary>
        /// 表示多结果集的谓词组的项目
        /// </summary>
        public class GetMultiplePredicateItem
        {
            /// <summary>
            /// 谓词组值对象
            /// </summary>
            public object Value { get; set; }

            /// <summary>
            /// 结果集实体类型
            /// </summary>
            public Type Type { get; set; }

            /// <summary>
            /// 排序条件列表
            /// </summary>
            public IList<ISort> Sort { get; set; }
        }
    }
}