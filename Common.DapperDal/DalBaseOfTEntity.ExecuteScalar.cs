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
        /// 执行SQL语句，返回第一行第一列数据
        /// </summary>
        /// <typeparam name="TAny">返回数据类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <returns>第一行第一列数据</returns>
        public virtual TAny ExecuteScalar<TAny>(string sql)
        {
            using (var connection = OpenConnection())
            {
                return connection.ExecuteScalar<TAny>(sql);
            }
        }

        /// <summary>
        /// 执行参数化SQL语句，返回第一行第一列数据
        /// </summary>
        /// <typeparam name="TAny">返回数据类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>第一行第一列数据</returns>
        public virtual TAny ExecuteScalar<TAny>(string sql, object parameters)
        {
            using (var connection = OpenConnection())
            {
                return connection.ExecuteScalar<TAny>(sql, parameters);
            }
        }

        /// <summary>
        /// 执行参数化SQL语句，返回第一行第一列数据
        /// </summary>
        /// <typeparam name="TAny">返回数据类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>第一行第一列数据</returns>
        public virtual TAny ExecuteScalar<TAny>(string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection())
            {
                return connection.ExecuteScalar<TAny>(sql, parameters, commandType: commandType);
            }
        }
    }
}
