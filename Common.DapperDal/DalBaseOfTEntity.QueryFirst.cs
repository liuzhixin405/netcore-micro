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
        /// 使用SQL语句获取实体
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>实体</returns>
        public virtual Task<TEntity> QueryFirst(string sql)
        {
            using (var connection = OpenConnection())
            {
                return connection.QueryFirstOrDefaultAsync<TEntity>(sql);
            }
        }

        /// <summary>
        /// 使用SQL语句获取实体
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>实体</returns>
        public virtual Task<TEntity> QueryFirst(string sql, object parameters)
        {
            using (var connection = OpenConnection())
            {
                return connection.QueryFirstOrDefaultAsync<TEntity>(sql, parameters);
            }
        }

        /// <summary>
        /// 使用SQL语句获取实体
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>实体</returns>
        public virtual Task<TEntity> QueryFirst(string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection())
            {
                return connection.QueryFirstOrDefaultAsync<TEntity>(sql, parameters, commandType: commandType);
            }
        }

        /// <summary>
        /// 使用SQL语句获取指定实体
        /// </summary>
        /// <typeparam name="TAny">返回实体类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <returns>实体</returns>
        public virtual Task<TAny> QueryFirst<TAny>(string sql)
        {
            using (var connection = OpenConnection())
            {
                return connection.QueryFirstOrDefaultAsync<TAny>(sql);
            }
        }

        /// <summary>
        /// 使用SQL语句获取指定实体
        /// </summary>
        /// <typeparam name="TAny">返回实体类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>实体</returns>
        public virtual Task<TAny> QueryFirst<TAny>(string sql, object parameters)
        {
            using (var connection = OpenConnection())
            {
                return connection.QueryFirstOrDefaultAsync<TAny>(sql, parameters);
            }
        }

        /// <summary>
        /// 使用SQL语句获取指定实体
        /// </summary>
        /// <typeparam name="TAny">返回实体类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>实体</returns>
        public virtual Task<TAny> QueryFirst<TAny>(string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection())
            {
                return connection.QueryFirstOrDefaultAsync<TAny>(sql, parameters, commandType: commandType);
            }
        }
    }
}
