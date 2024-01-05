using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DapperDal
{
    public partial class DalBase<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 根据实体ID（主键）获取实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>实体</returns>
        public virtual TEntity Get(TPrimaryKey id)
        {
            using (var connection = OpenConnection())
            {
                return Configuration.DalImplementor.Get<TEntity>(
                           connection: connection,
                           id: id,
                           transaction: null,
                           commandTimeout: null);
            }
        }
    }
}
