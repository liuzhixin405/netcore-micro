using System.Data;
using Dapper;

namespace DapperDal
{
    public partial class DalBase<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 执行 SQL 语句返回 DataSet
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataSet</returns>
        public virtual async Task<DataSet> QueryDataSet(string sql)
        {
            using (var connection = OpenConnection())
            {
                using (var reader =await connection.ExecuteReaderAsync(sql))
                {
                    DataSet ds = new DataSet();

                    while (!reader.IsClosed)
                    {
                        ds.Tables.Add().Load(reader);
                    }

                    return ds;
                }
            }
        }

        /// <summary>
        /// 执行 SQL 语句返回 DataSet
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>DataSet</returns>
        public virtual async Task<DataSet> QueryDataSet(string sql, object parameters)
        {
            using (var connection = OpenConnection())
            {
                using (var reader =await connection.ExecuteReaderAsync(sql, parameters))
                {
                    DataSet ds = new DataSet();

                    while (!reader.IsClosed)
                    {
                        ds.Tables.Add().Load(reader);
                    }

                    return ds;
                }
            }
        }

        /// <summary>
        /// 执行 SQL 语句返回 DataSet
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="commandType">SQL语句命令类型</param>
        /// <returns>DataSet</returns>
        public virtual async Task<DataSet> QueryDataSet(string sql, object parameters, CommandType commandType)
        {
            using (var connection = OpenConnection())
            {
                using (var reader =await connection.ExecuteReaderAsync(sql, parameters, commandType: commandType))
                {
                    DataSet ds = new DataSet();

                    while (!reader.IsClosed)
                    {
                        ds.Tables.Add().Load(reader);
                    }

                    return ds;
                }
            }
        }
    }
}
