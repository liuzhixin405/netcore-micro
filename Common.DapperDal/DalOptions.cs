using System;

namespace DapperDal
{
    /// <summary>
    /// 数据访问设置项
    /// </summary>
    public class DalOptions
    {
        /// <summary>
        /// 逻辑删除时更新属性和值的构造器
        /// </summary>
        public Func<object> SoftDeletePropsFactory { get; set; }

        /// <summary>
        /// 逻辑激活时更新属性和值的构造器
        /// </summary>
        public Func<object> SoftActivePropsFactory { get; set; }
    }
}
