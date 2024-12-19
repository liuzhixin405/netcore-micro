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
        /// <param name="sql">SQL语句</param>
        /// <returns>影响行数</returns>
        public virtual Task<int> Execute(string sql)
        {
            using (var connection = OpenConnection())
            {
                return connection.ExecuteAsync(sql);
            }
        }

        /// <summary>
        /// 执行参数化SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>影响行数</returns>
        public virtual Task<int> Execute(string sql, object parameters)
        {
            using (var connection = OpenConnection())
            {
                return connection.ExecuteAsync(sql, parameters);
            }
        }

        /// <summary>
        /// 执行参数化SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>影响行数</returns>
        public virtual Task<int> Execute(string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection())
            {
                return connection.ExecuteAsync(sql, parameters, commandType: commandType);
            }
        }
    }
}
