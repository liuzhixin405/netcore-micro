using System;

namespace DapperDal.Mapper
{
    /// <summary>
    /// Automatically maps an entity to a table using a combination of reflection and naming conventions for keys.
    /// </summary>
    public class AutoClassMapper<T> : ClassMapper<T> where T : class
    {
        /// <summary>
        /// 初始化自动类型绑定器
        /// </summary>
        public AutoClassMapper()
        {
            Type type = typeof(T);
            Table(type.Name);
            AutoMap();
        }
    }
}