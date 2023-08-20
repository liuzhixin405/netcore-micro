using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Dapper;

namespace DapperDal
{
    public partial class DalBase<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>
            QueryMultiple<TFirst, TSecond>(string connNameOrConnStr, string sql)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>
            QueryMultiple<TFirst, TSecond>(string connNameOrConnStr, string sql, object parameters)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql, parameters))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>
            QueryMultiple<TFirst, TSecond>(string connNameOrConnStr, string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql, parameters, commandType: commandType))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>>
            QueryMultiple<TFirst, TSecond, TThird>(string connNameOrConnStr, string sql)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>>
            QueryMultiple<TFirst, TSecond, TThird>(string connNameOrConnStr, string sql, object parameters)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql, parameters))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>>
            QueryMultiple<TFirst, TSecond, TThird>(string connNameOrConnStr, string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql, parameters, commandType: commandType))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth>(string connNameOrConnStr, string sql)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth>(string connNameOrConnStr, string sql, object parameters)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql, parameters))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth>(string connNameOrConnStr, string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql, parameters, commandType: commandType))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <typeparam name="TFifth">第五个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TFifth>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth, TFifth>(string connNameOrConnStr, string sql)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList(),
                        (IEnumerable<TFifth>)gridReader.Read<TFifth>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <typeparam name="TFifth">第五个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TFifth>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth, TFifth>(string connNameOrConnStr, string sql, object parameters)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql, parameters))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList(),
                        (IEnumerable<TFifth>)gridReader.Read<TFifth>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <typeparam name="TFifth">第五个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TFifth>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth, TFifth>(string connNameOrConnStr, string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql, parameters, commandType: commandType))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList(),
                        (IEnumerable<TFifth>)gridReader.Read<TFifth>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <typeparam name="TFifth">第五个实体类型</typeparam>
        /// <typeparam name="TSixth">第六个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TFifth>, IEnumerable<TSixth>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(string connNameOrConnStr, string sql)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList(),
                        (IEnumerable<TFifth>)gridReader.Read<TFifth>().ToList(),
                        (IEnumerable<TSixth>)gridReader.Read<TSixth>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <typeparam name="TFifth">第五个实体类型</typeparam>
        /// <typeparam name="TSixth">第六个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TFifth>, IEnumerable<TSixth>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(string connNameOrConnStr, string sql, object parameters)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql, parameters))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList(),
                        (IEnumerable<TFifth>)gridReader.Read<TFifth>().ToList(),
                        (IEnumerable<TSixth>)gridReader.Read<TSixth>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <typeparam name="TFifth">第五个实体类型</typeparam>
        /// <typeparam name="TSixth">第六个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TFifth>, IEnumerable<TSixth>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(string connNameOrConnStr, string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql, parameters, commandType: commandType))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList(),
                        (IEnumerable<TFifth>)gridReader.Read<TFifth>().ToList(),
                        (IEnumerable<TSixth>)gridReader.Read<TSixth>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <typeparam name="TFifth">第五个实体类型</typeparam>
        /// <typeparam name="TSixth">第六个实体类型</typeparam>
        /// <typeparam name="TSeventh">第七个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TFifth>, IEnumerable<TSixth>, IEnumerable<TSeventh>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(string connNameOrConnStr, string sql)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList(),
                        (IEnumerable<TFifth>)gridReader.Read<TFifth>().ToList(),
                        (IEnumerable<TSixth>)gridReader.Read<TSixth>().ToList(),
                        (IEnumerable<TSeventh>)gridReader.Read<TSeventh>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <typeparam name="TFifth">第五个实体类型</typeparam>
        /// <typeparam name="TSixth">第六个实体类型</typeparam>
        /// <typeparam name="TSeventh">第七个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TFifth>, IEnumerable<TSixth>, IEnumerable<TSeventh>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(string connNameOrConnStr, string sql, object parameters)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql, parameters))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList(),
                        (IEnumerable<TFifth>)gridReader.Read<TFifth>().ToList(),
                        (IEnumerable<TSixth>)gridReader.Read<TSixth>().ToList(),
                        (IEnumerable<TSeventh>)gridReader.Read<TSeventh>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <typeparam name="TFifth">第五个实体类型</typeparam>
        /// <typeparam name="TSixth">第六个实体类型</typeparam>
        /// <typeparam name="TSeventh">第七个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TFifth>, IEnumerable<TSixth>, IEnumerable<TSeventh>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(string connNameOrConnStr, string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql, parameters, commandType: commandType))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList(),
                        (IEnumerable<TFifth>)gridReader.Read<TFifth>().ToList(),
                        (IEnumerable<TSixth>)gridReader.Read<TSixth>().ToList(),
                        (IEnumerable<TSeventh>)gridReader.Read<TSeventh>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <typeparam name="TFifth">第五个实体类型</typeparam>
        /// <typeparam name="TSixth">第六个实体类型</typeparam>
        /// <typeparam name="TSeventh">第七个实体类型</typeparam>
        /// <typeparam name="TEighth">第八个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TFifth>, IEnumerable<TSixth>, IEnumerable<TSeventh>, Tuple<IEnumerable<TEighth>>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(string connNameOrConnStr, string sql)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList(),
                        (IEnumerable<TFifth>)gridReader.Read<TFifth>().ToList(),
                        (IEnumerable<TSixth>)gridReader.Read<TSixth>().ToList(),
                        (IEnumerable<TSeventh>)gridReader.Read<TSeventh>().ToList(),
                        (IEnumerable<TEighth>)gridReader.Read<TEighth>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <typeparam name="TFifth">第五个实体类型</typeparam>
        /// <typeparam name="TSixth">第六个实体类型</typeparam>
        /// <typeparam name="TSeventh">第七个实体类型</typeparam>
        /// <typeparam name="TEighth">第八个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TFifth>, IEnumerable<TSixth>, IEnumerable<TSeventh>, Tuple<IEnumerable<TEighth>>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(string connNameOrConnStr, string sql, object parameters)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql, parameters))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList(),
                        (IEnumerable<TFifth>)gridReader.Read<TFifth>().ToList(),
                        (IEnumerable<TSixth>)gridReader.Read<TSixth>().ToList(),
                        (IEnumerable<TSeventh>)gridReader.Read<TSeventh>().ToList(),
                        (IEnumerable<TEighth>)gridReader.Read<TEighth>().ToList());
                }
            }
        }

        /// <summary>
        /// 使用SQL语句获取多个指定实体集合
        /// </summary>
        /// <typeparam name="TFirst">第一个实体类型</typeparam>
        /// <typeparam name="TSecond">第二个实体类型</typeparam>
        /// <typeparam name="TThird">第三个实体类型</typeparam>
        /// <typeparam name="TFourth">第四个实体类型</typeparam>
        /// <typeparam name="TFifth">第五个实体类型</typeparam>
        /// <typeparam name="TSixth">第六个实体类型</typeparam>
        /// <typeparam name="TSeventh">第七个实体类型</typeparam>
        /// <typeparam name="TEighth">第八个实体类型</typeparam>
        /// <param name="connNameOrConnStr">DB 连接字符串配置节点名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>多个实体集合</returns>
        public virtual Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TFifth>, IEnumerable<TSixth>, IEnumerable<TSeventh>, Tuple<IEnumerable<TEighth>>>
            QueryMultiple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(string connNameOrConnStr, string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection(connNameOrConnStr))
            {
                using (var gridReader = connection.QueryMultiple(sql, parameters, commandType: commandType))
                {
                    return Tuple.Create((IEnumerable<TFirst>)gridReader.Read<TFirst>().ToList(),
                        (IEnumerable<TSecond>)gridReader.Read<TSecond>().ToList(),
                        (IEnumerable<TThird>)gridReader.Read<TThird>().ToList(),
                        (IEnumerable<TFourth>)gridReader.Read<TFourth>().ToList(),
                        (IEnumerable<TFifth>)gridReader.Read<TFifth>().ToList(),
                        (IEnumerable<TSixth>)gridReader.Read<TSixth>().ToList(),
                        (IEnumerable<TSeventh>)gridReader.Read<TSeventh>().ToList(),
                        (IEnumerable<TEighth>)gridReader.Read<TEighth>().ToList());
                }
            }
        }
    }
}
