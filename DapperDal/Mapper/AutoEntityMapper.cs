using System;

namespace DapperDal.Mapper
{
    /// <summary>
    /// 实体自动映射器类
    /// </summary>
    /// <typeparam name="T">实体类</typeparam>
    public class AutoEntityMapper<T> : ClassMapper<T> where T : class
    {
        /// <summary>
        /// 初始化自动映射实体
        /// </summary>
        public AutoEntityMapper()
        {
            Type type = typeof(T);
            Table(RemovePostFix(type.Name, "Entity"));
            AutoMap();
        }


        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="postFixes">one or more postfix.</param>
        /// <returns>Modified string or the same string if it has not any of given postfixes</returns>
        public static string RemovePostFix(string str, params string[] postFixes)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            if (postFixes == null || postFixes.Length == 0)
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (!string.IsNullOrEmpty(postFix) && str.EndsWith(postFix))
                {
                    return str.Substring(0, str.Length - postFix.Length);
                }
            }

            return str;
        }
    }
}
