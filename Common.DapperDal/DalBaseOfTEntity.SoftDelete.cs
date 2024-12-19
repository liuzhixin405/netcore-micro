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
        /// 逻辑删除指定实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="props">逻辑删除属性名及更新值，默认 IsActive=0</param>
        /// <returns>逻辑删除结果</returns>
        public virtual Task<bool> SoftDelete(TEntity entity, object props = null)
        {
            return SwitchActive(entity, false, props);
        }

        /// <summary>
        /// 根据实体主键ID逻辑删除指定实体
        /// </summary>
        /// <param name="id">实体主键ID</param>
        /// <param name="props">逻辑删除属性名及更新值，默认 IsActive=0</param>
        /// <returns>逻辑删除结果</returns>
        public virtual Task<bool> SoftDelete(TPrimaryKey id, object props = null)
        {
            return SwitchActive(id, false, props);
        }

        /// <summary>
        /// 根据条件逻辑删除实体
        /// </summary>
        /// <param name="predicate">删除条件</param>
        /// <param name="props">逻辑删除属性名及更新值，默认 IsActive=0</param>
        /// <returns>逻辑删除结果</returns>
        public virtual Task<bool> SoftDelete(object predicate, object props = null)
        {
            return SwitchActive(predicate, false, props);
        }

        /// <summary>
        /// 根据条件逻辑删除实体
        /// </summary>
        /// <param name="predicate">删除条件</param>
        /// <param name="props">逻辑删除属性名及更新值，默认 IsActive=0 }</param>
        /// <returns>逻辑删除结果</returns>
        public virtual Task<bool> SoftDelete(Expression<Func<TEntity, bool>> predicate, object props = null)
        {
            return SwitchActive(predicate, false, props);
        }

    }
}
