using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DapperDal.Utils
{
    /// <summary>
    /// 反射工具方法
    /// </summary>
    internal static class ReflectionHelper
    {
        private static List<Type> _simpleTypes = new List<Type>
                               {
                                   typeof(byte),
                                   typeof(sbyte),
                                   typeof(short),
                                   typeof(ushort),
                                   typeof(int),
                                   typeof(uint),
                                   typeof(long),
                                   typeof(ulong),
                                   typeof(float),
                                   typeof(double),
                                   typeof(decimal),
                                   typeof(bool),
                                   typeof(string),
                                   typeof(char),
                                   typeof(Guid),
                                   typeof(DateTime),
                                   typeof(DateTimeOffset),
                                   typeof(byte[])
                               };

        /// <summary>
        /// 从表达式获取成员元数据访问器对象
        /// </summary>
        /// <param name="lambda">表达式</param>
        /// <returns>成员元数据访问器对象</returns>
        public static MemberInfo GetProperty(LambdaExpression lambda)
        {
            Expression expr = lambda;
            for (; ; )
            {
                switch (expr.NodeType)
                {
                    case ExpressionType.Lambda:
                        expr = ((LambdaExpression)expr).Body;
                        break;
                    case ExpressionType.Convert:
                        expr = ((UnaryExpression)expr).Operand;
                        break;
                    case ExpressionType.MemberAccess:
                        MemberExpression memberExpression = (MemberExpression)expr;
                        MemberInfo mi = memberExpression.Member;
                        return mi;
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// 对象生成属性名及属性值的字典返回
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>属性名及属性值的字典</returns>
        public static IDictionary<string, object> GetObjectValues(object obj)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            if (obj == null)
            {
                return result;
            }


            foreach (var propertyInfo in obj.GetType().GetProperties())
            {
                string name = propertyInfo.Name;
                object value = propertyInfo.GetValue(obj, null);
                result[name] = value;
            }

            return result;
        }

        /// <summary>
        /// 拼接查询字段名
        /// </summary>
        /// <param name="list">查询字段名列表</param>
        /// <param name="seperator">拼接分割字符</param>
        /// <returns>拼接后的语句</returns>
        public static string AppendStrings(this IEnumerable<string> list, string seperator = ", ")
        {
            return list.Aggregate(
                new StringBuilder(),
                (sb, s) => (sb.Length == 0 ? sb : sb.Append(seperator)).Append(s),
                sb => sb.ToString());
        }

        /// <summary>
        /// 类型是否是简单类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否是简单类型</returns>
        public static bool IsSimpleType(Type type)
        {
            Type actualType = type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                actualType = type.GetGenericArguments()[0];
            }

            return _simpleTypes.Contains(actualType);
        }

        /// <summary>
        /// 生成SQL参数片段
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <param name="parameterName">参数名</param>
        /// <param name="parameterPrefix">参数前缀</param>
        /// <returns>参数片段</returns>
        public static string GetParameterName(this IDictionary<string, object> parameters, string parameterName, char parameterPrefix)
        {
            return string.Format("{0}{1}_{2}", parameterPrefix, parameterName, parameters.Count);
        }

        /// <summary>
        /// 生成SQL参数片段
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="parameterPrefix">参数前缀</param>
        /// <returns>参数片段</returns>
        public static string SetParameterName(this IDictionary<string, object> parameters, string parameterName, object value, char parameterPrefix)
        {
            string name = parameters.GetParameterName(parameterName, parameterPrefix);
            parameters.Add(name, value);
            return name;
        }
    }
}