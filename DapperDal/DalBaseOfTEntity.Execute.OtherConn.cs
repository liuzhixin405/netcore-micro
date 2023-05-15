using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;

namespace DapperDal
{
    public partial class DalBase<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <returns>影响行数</returns>
        public virtual int Execute(string connNameOrConnStr, string sql)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                return connection.Execute(sql);
            }
        }

        /// <summary>
        /// 执行参数化SQL语句
        /// </summary>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>影响行数</returns>
        public virtual int Execute(string connNameOrConnStr, string sql, object parameters)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                return connection.Execute(sql, parameters);
            }
        }

        /// <summary>
        /// 执行参数化SQL语句
        /// </summary>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>影响行数</returns>
        public virtual int Execute(string connNameOrConnStr, string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                return connection.Execute(sql, parameters, commandType: commandType);
            }
        }
    }
}
