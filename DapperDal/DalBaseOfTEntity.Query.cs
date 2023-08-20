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
        /// <param name="sql">SQL语句</param>
        /// <returns>实体集合</returns>
        public virtual IEnumerable<TEntity> Query(string sql)
        {
            using (var connection = OpenConnection())
            {
                return connection.Query<TEntity>(sql);
            }
        }

        /// <summary>
        /// 使用SQL语句获取实体集合
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>实体集合</returns>
        public virtual IEnumerable<TEntity> Query(string sql, object parameters)
        {
            using (var connection = OpenConnection())
            {
                return connection.Query<TEntity>(sql, parameters);
            }
        }

        /// <summary>
        /// 使用SQL语句获取实体集合
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>实体集合</returns>
        public virtual IEnumerable<TEntity> Query(string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection())
            {
                return connection.Query<TEntity>(sql, parameters, commandType: commandType);
            }
        }

        /// <summary>
        /// 使用SQL语句获取指定实体集合
        /// </summary>
        /// <typeparam name="TAny">返回实体类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <returns>实体集合</returns>
        public virtual IEnumerable<TAny> Query<TAny>(string sql)
        {
            using (var connection = OpenConnection())
            {
                return connection.Query<TAny>(sql);
            }
        }

        /// <summary>
        /// 使用SQL语句获取指定实体集合
        /// </summary>
        /// <typeparam name="TAny">返回实体类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>实体集合</returns>
        public virtual IEnumerable<TAny> Query<TAny>(string sql, object parameters)
        {
            using (var connection = OpenConnection())
            {
                return connection.Query<TAny>(sql, parameters);
            }
        }

        /// <summary>
        /// 使用SQL语句获取指定实体集合
        /// </summary>
        /// <typeparam name="TAny">返回实体类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>实体集合</returns>
        public virtual IEnumerable<TAny> Query<TAny>(string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection())
            {
                return connection.Query<TAny>(sql, parameters, commandType: commandType);
            }
        }
    }
}
