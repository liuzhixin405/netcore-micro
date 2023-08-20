using System;

namespace DapperDal.Expressions
{
    /// <summary>
    /// 映射SQL查询支持的函数
    /// </summary>
    public static class QueryFunctions
    {
        /// <summary>
        /// For reflection only.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool Like(string pattern, object member)
        {
            throw new InvalidOperationException("For reflection only!");
        }
    }
}
