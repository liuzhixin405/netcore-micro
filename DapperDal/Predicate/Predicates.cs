using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using DapperDal.Mapper;
using DapperDal.Sql;
using DapperDal.Utils;

namespace DapperDal.Predicate
{
    /// <summary>
    /// 谓词组工具方法
    /// </summary>
    public static class Predicates
    {
        /// <summary>
        /// Factory method that creates a new IFieldPredicate predicate: [FieldName] [Operator] [Value]. 
        /// Example: WHERE FirstName = 'Foo'
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="expression">An expression that returns the left operand [FieldName].</param>
        /// <param name="op">The comparison operator.</param>
        /// <param name="value">The value for the predicate.</param>
        /// <param name="not">Effectively inverts the comparison operator. Example: WHERE FirstName &lt;&gt; 'Foo'.</param>
        /// <returns>An instance of IFieldPredicate.</returns>
        public static IFieldPredicate Field<T>(Expression<Func<T, object>> expression, Operator op, object value, bool not = false) where T : class
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            return new FieldPredicate<T>
                       {
                           PropertyName = propertyInfo.Name,
                           Operator = op,
                           Value = value,
                           Not = not
                       };
        }

        /// <summary>
        /// Factory method that creates a new IPropertyPredicate predicate: [FieldName1] [Operator] [FieldName2]
        /// Example: WHERE FirstName = LastName
        /// </summary>
        /// <typeparam name="T">The type of the entity for the left operand.</typeparam>
        /// <typeparam name="T2">The type of the entity for the right operand.</typeparam>
        /// <param name="expression">An expression that returns the left operand [FieldName1].</param>
        /// <param name="op">The comparison operator.</param>
        /// <param name="expression2">An expression that returns the right operand [FieldName2].</param>
        /// <param name="not">Effectively inverts the comparison operator. Example: WHERE FirstName &lt;&gt; LastName </param>
        /// <returns>An instance of IPropertyPredicate.</returns>
        public static IPropertyPredicate Property<T, T2>(Expression<Func<T, object>> expression, Operator op, Expression<Func<T2, object>> expression2, bool not = false)
            where T : class
            where T2 : class
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            PropertyInfo propertyInfo2 = ReflectionHelper.GetProperty(expression2) as PropertyInfo;
            return new PropertyPredicate<T, T2>
                       {
                           PropertyName = propertyInfo.Name,
                           PropertyName2 = propertyInfo2.Name,
                           Operator = op,
                           Not = not
                       };
        }

        /// <summary>
        /// Factory method that creates a new IPredicateGroup predicate.
        /// Predicate groups can be joined together with other predicate groups.
        /// </summary>
        /// <param name="op">The grouping operator to use when joining the predicates (AND / OR).</param>
        /// <param name="predicate">A list of predicates to group.</param>
        /// <returns>An instance of IPredicateGroup.</returns>
        public static IPredicateGroup Group(GroupOperator op, params IPredicate[] predicate)
        {
            return new PredicateGroup
                       {
                           Operator = op,
                           Predicates = predicate
                       };
        }

        /// <summary>
        /// Factory method that creates a new IExistsPredicate predicate.
        /// </summary>
        public static IExistsPredicate Exists<TSub>(IPredicate predicate, bool not = false)
            where TSub : class
        {
            return new ExistsPredicate<TSub>
                       {
                           Not = not,
                           Predicate = predicate
                       };
        }

        /// <summary>
        /// Factory method that creates a new IBetweenPredicate predicate. 
        /// </summary>
        public static IBetweenPredicate Between<T>(Expression<Func<T, object>> expression, BetweenValues values, bool not = false)
            where T : class
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            return new BetweenPredicate<T>
                       {
                           Not = not,
                           PropertyName = propertyInfo.Name,
                           Value = values
                       };
        }

        /// <summary>
        /// Factory method that creates a new Sort which controls how the results will be sorted.
        /// </summary>
        public static ISort Sort<T>(Expression<Func<T, object>> expression, bool ascending = true)
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            return new Sort
                       {
                           PropertyName = propertyInfo.Name,
                           Ascending = ascending
                       };
        }
    }

    /// <summary>
    /// 表示谓词的接口
    /// </summary>
    public interface IPredicate
    {
        /// <summary>
        /// 返回根据参数使用SQL生成器生成的SQL语句
        /// </summary>
        /// <param name="sqlGenerator">SQL生成器</param>
        /// <param name="parameters">参数</param>
        /// <returns>SQL语句</returns>
        string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters);
    }

    /// <summary>
    /// 表示谓词的基接口
    /// </summary>
    public interface IBasePredicate : IPredicate
    {
        /// <summary>
        /// 属性名
        /// </summary>
        string PropertyName { get; set; }
    }

    /// <summary>
    /// 谓词基类
    /// </summary>
    public abstract class BasePredicate : IBasePredicate
    {
        /// <inheritdoc />
        public abstract string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters);

        /// <inheritdoc />
        public string PropertyName { get; set; }

        /// <summary>
        /// 获取字段名
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="sqlGenerator">SQL生成器</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>字段名</returns>
        /// <exception cref="NullReferenceException">实体类型或属性映射未找到</exception>
        protected virtual string GetColumnName(Type entityType, ISqlGenerator sqlGenerator, string propertyName)
        {
            IClassMapper map = sqlGenerator.Configuration.GetMap(entityType);
            if (map == null)
            {
                throw new NullReferenceException(string.Format("Map was not found for {0}", entityType));
            }

            IPropertyMap propertyMap = map.Properties.SingleOrDefault(p => p.Name == propertyName);
            if (propertyMap == null)
            {
                throw new NullReferenceException(string.Format("{0} was not found for {1}", propertyName, entityType));
            }

            return sqlGenerator.GetColumnName(map, propertyMap, false);
        }
    }

    /// <summary>
    /// 表示比较的谓词接口
    /// </summary>
    public interface IComparePredicate : IBasePredicate
    {
        /// <summary>
        /// 比较操作类型
        /// </summary>
        Operator Operator { get; set; }

        /// <summary>
        /// 比较类型是否逻辑非
        /// </summary>
        bool Not { get; set; }
    }

    /// <summary>
    /// 比较谓词基类
    /// </summary>
    public abstract class ComparePredicate : BasePredicate
    {
        /// <summary>
        /// 比较操作类型
        /// </summary>
        public Operator Operator { get; set; }

        /// <summary>
        /// 比较类型是否逻辑非
        /// </summary>
        public bool Not { get; set; }

        /// <summary>
        /// 获取比较操作类型的SQL片段
        /// </summary>
        /// <returns>SQL片段</returns>
        public virtual string GetOperatorString()
        {
            switch (Operator)
            {
                case Operator.Gt:
                    return Not ? "<=" : ">";
                case Operator.Ge:
                    return Not ? "<" : ">=";
                case Operator.Lt:
                    return Not ? ">=" : "<";
                case Operator.Le:
                    return Not ? ">" : "<=";
                case Operator.Like:
                    return Not ? "NOT LIKE" : "LIKE";
                default:
                    return Not ? "<>" : "=";
            }
        }
    }

    /// <summary>
    /// 表示字段值比较谓的词组接口
    /// </summary>
    public interface IFieldPredicate : IComparePredicate
    {
        /// <summary>
        /// 字段值
        /// </summary>
        object Value { get; set; }
    }

    /// <summary>
    /// 字段值比较谓词类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FieldPredicate<T> : ComparePredicate, IFieldPredicate
        where T : class
    {
        /// <inheritdoc />
        public object Value { get; set; }

        /// <inheritdoc />
        public override string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            string columnName = GetColumnName(typeof(T), sqlGenerator, PropertyName);
            if (Value == null)
            {
                return string.Format("({0} IS {1}NULL)", columnName, Not ? "NOT " : string.Empty);
            }

            if (Value is IEnumerable && !(Value is string))
            {
                if (Operator != Operator.Eq)
                {
                    throw new ArgumentException("Operator must be set to Eq for Enumerable types");
                }

                List<string> @params = new List<string>();
                foreach (var value in (IEnumerable)Value)
                {
                    string valueParameterName = parameters.SetParameterName(this.PropertyName, value, sqlGenerator.Configuration.Dialect.ParameterPrefix);
                    @params.Add(valueParameterName);
                }

                string paramStrings = @params.Aggregate(new StringBuilder(), (sb, s) => sb.Append((sb.Length != 0 ? ", " : string.Empty) + s), sb => sb.ToString());
                return string.Format("({0} {1}IN ({2}))", columnName, Not ? "NOT " : string.Empty, paramStrings);
            }

            string parameterName = parameters.SetParameterName(this.PropertyName, this.Value, sqlGenerator.Configuration.Dialect.ParameterPrefix);
            return string.Format("({0} {1} {2})", columnName, GetOperatorString(), parameterName);
        }
    }

    /// <summary>
    /// 表示两个属性比较的谓词接口
    /// </summary>
    public interface IPropertyPredicate : IComparePredicate
    {
        /// <summary>
        /// 第二个属性名
        /// </summary>
        string PropertyName2 { get; set; }
    }

    /// <summary>
    /// 两个属性比较的谓词类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class PropertyPredicate<T, T2> : ComparePredicate, IPropertyPredicate
        where T : class
        where T2 : class
    {
        /// <inheritdoc />
        public string PropertyName2 { get; set; }

        /// <inheritdoc />
        public override string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            string columnName = GetColumnName(typeof(T), sqlGenerator, PropertyName);
            string columnName2 = GetColumnName(typeof(T2), sqlGenerator, PropertyName2);
            return string.Format("({0} {1} {2})", columnName, GetOperatorString(), columnName2);
        }
    }

    /// <summary>
    /// 表示在两者之间的起始值、结束值的结构
    /// </summary>
    public struct BetweenValues
    {
        /// <summary>
        /// 起始值
        /// </summary>
        public object Value1 { get; set; }

        /// <summary>
        /// 结束值
        /// </summary>
        public object Value2 { get; set; }
    }

    /// <summary>
    /// 表示属性在两者之间的谓词接口
    /// </summary>
    public interface IBetweenPredicate : IPredicate
    {
        /// <summary>
        /// 属性名
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// 起始值、结束值
        /// </summary>
        BetweenValues Value { get; set; }

        /// <summary>
        /// 比较类型是否逻辑非
        /// </summary>
        bool Not { get; set; }

    }

    /// <summary>
    /// 表示属性在两者之间的谓词类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BetweenPredicate<T> : BasePredicate, IBetweenPredicate
        where T : class
    {
        /// <inheritdoc />
        public override string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            string columnName = GetColumnName(typeof(T), sqlGenerator, PropertyName);
            string propertyName1 = parameters.SetParameterName(this.PropertyName, this.Value.Value1, sqlGenerator.Configuration.Dialect.ParameterPrefix);
            string propertyName2 = parameters.SetParameterName(this.PropertyName, this.Value.Value2, sqlGenerator.Configuration.Dialect.ParameterPrefix);
            return string.Format("({0} {1}BETWEEN {2} AND {3})", columnName, Not ? "NOT " : string.Empty, propertyName1, propertyName2);
        }

        /// <inheritdoc />
        public BetweenValues Value { get; set; }

        /// <inheritdoc />
        public bool Not { get; set; }
    }

    /// <summary>
    /// 谓词比较操作类型
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// 等于
        /// Equal to
        /// </summary>
        Eq,

        /// <summary>
        /// 大于
        /// Greater than
        /// </summary>
        Gt,

        /// <summary>
        /// 大于等于
        /// Greater than or equal to
        /// </summary>
        Ge,

        /// <summary>
        /// 小于
        /// Less than
        /// </summary>
        Lt,

        /// <summary>
        /// 小于等于
        /// Less than or equal to
        /// </summary>
        Le,

        /// <summary>
        /// 类似于
        /// Like (You can use % in the value to do wilcard searching)
        /// </summary>
        Like
    }

    /// <summary>
    /// 表示谓词组的接口
    /// </summary>
    public interface IPredicateGroup : IPredicate
    {
        /// <summary>
        /// 谓词之间组合的操作类型
        /// </summary>
        GroupOperator Operator { get; set; }

        /// <summary>
        /// 谓词组
        /// </summary>
        IList<IPredicate> Predicates { get; set; }
    }

    /// <summary>
    /// 谓词组类
    /// </summary>
    public class PredicateGroup : IPredicateGroup
    {
        /// <inheritdoc />
        public GroupOperator Operator { get; set; }

        /// <inheritdoc />
        public IList<IPredicate> Predicates { get; set; }

        /// <inheritdoc />
        public string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            string seperator = Operator == GroupOperator.And ? " AND " : " OR ";
            return "(" + Predicates.Aggregate(new StringBuilder(),
                                        (sb, p) => (sb.Length == 0 ? sb : sb.Append(seperator)).Append(p.GetSql(sqlGenerator, parameters)),
                sb =>
                {
                    var s = sb.ToString();
                    if (s.Length == 0) return sqlGenerator.Configuration.Dialect.EmptyExpression; 
                    return s;
                }
                                        ) + ")";
        }
    }

    /// <summary>
    /// 表示是否存在的谓词接口
    /// </summary>
    public interface IExistsPredicate : IPredicate
    {
        /// <summary>
        /// 子谓词
        /// </summary>
        IPredicate Predicate { get; set; }

        /// <summary>
        /// 比较类型是否逻辑非
        /// </summary>
        bool Not { get; set; }
    }

    /// <summary>
    /// 是否存在的谓词类
    /// </summary>
    /// <typeparam name="TSub"></typeparam>
    public class ExistsPredicate<TSub> : IExistsPredicate
        where TSub : class
    {
        /// <inheritdoc />
        public IPredicate Predicate { get; set; }

        /// <inheritdoc />
        public bool Not { get; set; }

        /// <inheritdoc />
        public string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            IClassMapper mapSub = GetClassMapper(typeof(TSub), sqlGenerator.Configuration);
            string sql = string.Format("({0}EXISTS (SELECT 1 FROM {1} WHERE {2}))",
                Not ? "NOT " : string.Empty,
                sqlGenerator.GetTableName(mapSub),
                Predicate.GetSql(sqlGenerator, parameters));
            return sql;
        }

        /// <summary>
        /// 获取实体类型的映射器
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="configuration">数据访问配置</param>
        /// <returns>实体类型的映射器</returns>
        /// <exception cref="NullReferenceException">实体类型的映射器未找到</exception>
        protected virtual IClassMapper GetClassMapper(Type type, IDalConfiguration configuration)
        {
            IClassMapper map = configuration.GetMap(type);
            if (map == null)
            {
                throw new NullReferenceException(string.Format("Map was not found for {0}", type));
            }

            return map;
        }
    }

    /// <summary>
    /// 表示排序条件的接口
    /// </summary>
    public interface ISort
    {
        /// <summary>
        /// 排序属性名
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// 是否升序
        /// </summary>
        bool Ascending { get; set; }
    }

    /// <summary>
    /// 排序条件
    /// </summary>
    public class Sort : ISort
    {
        /// <inheritdoc />
        public string PropertyName { get; set; }

        /// <inheritdoc />
        public bool Ascending { get; set; }
    }

    /// <summary>
    /// 谓词间组合操作类型
    /// </summary>
    public enum GroupOperator
    {
        /// <summary>
        /// 并且
        /// </summary>
        And,

        /// <summary>
        /// 或者
        /// </summary>
        Or
    }
}