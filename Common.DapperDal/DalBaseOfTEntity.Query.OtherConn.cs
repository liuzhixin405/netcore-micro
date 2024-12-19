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
        /// 使用SQL语句获取实体集合
        /// </summary>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <returns>实体集合</returns>
        public virtual Task<IEnumerable<TEntity>> Query(string connNameOrConnStr, string sql)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                return connection.QueryAsync<TEntity>(sql);
            }
        }

        /// <summary>
        /// 使用SQL语句获取实体集合
        /// </summary>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>实体集合</returns>
        public virtual Task<IEnumerable<TEntity>> Query(string connNameOrConnStr, string sql, object parameters)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                return connection.QueryAsync<TEntity>(sql, parameters);
            }
        }

        /// <summary>
        /// 使用SQL语句获取实体集合
        /// </summary>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>实体集合</returns>
        public virtual Task<IEnumerable<TEntity>> Query(string connNameOrConnStr, string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                return connection.QueryAsync<TEntity>(sql, parameters, commandType: commandType);
            }
        }

        /// <summary>
        /// 使用SQL语句获取指定实体集合
        /// </summary>
        /// <typeparam name="TAny">返回实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <returns>实体集合</returns>
        public virtual Task<IEnumerable<TAny>> Query<TAny>(string connNameOrConnStr, string sql)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                return connection.QueryAsync<TAny>(sql);
            }
        }

        /// <summary>
        /// 使用SQL语句获取指定实体集合
        /// </summary>
        /// <typeparam name="TAny">返回实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>实体集合</returns>
        public virtual Task<IEnumerable<TAny>> Query<TAny>(string connNameOrConnStr, string sql, object parameters)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                return connection.QueryAsync<TAny>(sql, parameters);
            }
        }

        /// <summary>
        /// 使用SQL语句获取指定实体集合
        /// </summary>
        /// <typeparam name="TAny">返回实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>实体集合</returns>
        public virtual Task<IEnumerable<TAny>> Query<TAny>(string connNameOrConnStr, string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                return connection.QueryAsync<TAny>(sql, parameters, commandType: commandType);
            }
        }
    }
}
