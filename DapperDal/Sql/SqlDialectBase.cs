using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperDal.Sql
{
    /// <summary>
    /// SQL方言接口
    /// </summary>
    public interface ISqlDialect
    {
        /// <summary>
        /// 表示左引号的字符
        /// </summary>
        char OpenQuote { get; }

        /// <summary>
        /// 表示右引号的字符
        /// </summary>
        char CloseQuote { get; }

        /// <summary>
        /// 表示批次分割的字符串
        /// </summary>
        string BatchSeperator { get; }

        /// <summary>
        /// 是否支持多语句执行
        /// </summary>
        bool SupportsMultipleStatements { get; }

        /// <summary>
        /// 表示参数前缀的字符
        /// </summary>
        char ParameterPrefix { get; }

        /// <summary>
        /// 表示空表达式的字符串
        /// </summary>
        string EmptyExpression { get; }

        /// <summary>
        /// 生成表名
        /// </summary>
        /// <param name="schemaName">数据库架构</param>
        /// <param name="tableName">数据库表名</param>
        /// <param name="alias">数据库表别名</param>
        /// <returns>表名</returns>
        string GetTableName(string schemaName, string tableName, string alias);

        /// <summary>
        /// 生成字段名
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="columnName">字段名</param>
        /// <param name="alias">字段别名</param>
        /// <returns>字段名</returns>
        string GetColumnName(string prefix, string columnName, string alias);

        /// <summary>
        /// 生成包含主键条件的获取语句
        /// </summary>
        /// <param name="tableName">数据库表名</param>
        /// <returns>SQL</returns>
        string GetIdentitySql(string tableName);

        /// <summary>
        /// 生成分页SQL
        /// </summary>
        /// <param name="sql">原SQL</param>
        /// <param name="page">页索引</param>
        /// <param name="resultsPerPage">每页条数</param>
        /// <param name="parameters">参数</param>
        /// <returns>新SQL</returns>
        string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters);

        /// <summary>
        /// 生成区间SQL
        /// </summary>
        /// <param name="sql">原SQL</param>
        /// <param name="firstResult">起始行数</param>
        /// <param name="maxResults">最大条数</param>
        /// <param name="parameters">参数</param>
        /// <returns>新SQL</returns>
        string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters);

        /// <summary>
        /// 判断语句是否包含引号
        /// </summary>
        /// <param name="value">语句</param>
        /// <returns>是否包含引号</returns>
        bool IsQuoted(string value);

        /// <summary>
        /// 语句添加引号
        /// </summary>
        /// <param name="value">原语句</param>
        /// <returns>新语句</returns>
        string QuoteString(string value);

        /// <summary>
        /// 获取前N条的SQL
        /// </summary>
        /// <param name="sql">原SQL</param>
        /// <param name="limit">指定前几条</param>
        /// <returns>新SQL</returns>
        string SelectLimit(string sql, int limit);

        /// <summary>
        /// SQL语句添加 WITH (NOLOCK)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>添加后SQL语句</returns>
        string SetNolock(string sql);
    }

    /// <summary>
    /// SQL方言基类
    /// </summary>
    public abstract class SqlDialectBase : ISqlDialect
    {
        /// <inheritdoc />
        public virtual char OpenQuote
        {
            get { return '"'; }
        }

        /// <inheritdoc />
        public virtual char CloseQuote
        {
            get { return '"'; }
        }

        /// <inheritdoc />
        public virtual string BatchSeperator
        {
            get { return ";" + Environment.NewLine; }
        }

        /// <inheritdoc />
        public virtual bool SupportsMultipleStatements
        {
            get { return true; }
        }

        /// <inheritdoc />
        public virtual char ParameterPrefix
        {
            get { return '@'; }
        }

        /// <inheritdoc />
        public string EmptyExpression
        {
            get { return "1=1"; }
        }

        /// <inheritdoc />
        public virtual string GetTableName(string schemaName, string tableName, string alias)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("TableName", "tableName cannot be null or empty.");
            }

            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(schemaName))
            {
                result.AppendFormat(QuoteString(schemaName) + ".");
            }

            result.AppendFormat(QuoteString(tableName));

            if (!string.IsNullOrWhiteSpace(alias))
            {
                result.AppendFormat(" AS {0}", QuoteString(alias));
            }
            return result.ToString();
        }

        /// <inheritdoc />
        public virtual string GetColumnName(string prefix, string columnName, string alias)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentNullException("ColumnName", "columnName cannot be null or empty.");
            }

            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                result.AppendFormat(QuoteString(prefix) + ".");
            }

            result.AppendFormat(QuoteString(columnName));

            if (!string.IsNullOrWhiteSpace(alias))
            {
                result.AppendFormat(" AS {0}", QuoteString(alias));
            }

            return result.ToString();
        }

        /// <inheritdoc />
        public abstract string GetIdentitySql(string tableName);

        /// <inheritdoc />
        public abstract string GetPagingSql(string sql, int page, int resultsPerPage,
            IDictionary<string, object> parameters);

        /// <inheritdoc />
        public abstract string GetSetSql(string sql, int firstResult, int maxResults,
            IDictionary<string, object> parameters);

        /// <inheritdoc />
        public virtual bool IsQuoted(string value)
        {
            if (value.Trim()[0] == OpenQuote)
            {
                return value.Trim().Last() == CloseQuote;
            }

            return false;
        }

        /// <inheritdoc />
        public virtual string QuoteString(string value)
        {
            if (IsQuoted(value) || value == "*")
            {
                return value;
            }
            return string.Format("{0}{1}{2}", OpenQuote, value.Trim(), CloseQuote);
        }

        /// <inheritdoc />
        public virtual string UnQuoteString(string value)
        {
            return IsQuoted(value) ? value.Substring(1, value.Length - 2) : value;
        }

        /// <inheritdoc />
        public virtual string SelectLimit(string sql, int limit)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public virtual string SetNolock(string sql)
        {
            throw new NotSupportedException();
        }
    }
}
