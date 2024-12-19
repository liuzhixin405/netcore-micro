using System;
using System.Linq.Expressions;
using DapperDal.Expressions;
using DapperDal.Predicate;

namespace DapperDal
{
    public partial class DalBase<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 逻辑删除或激活指定实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isActive">是否激活，true:激活，false:逻辑删除</param>
        /// <param name="softDeleteProps">逻辑删除属性名及更新值，默认:IsActive=0</param>
        /// <param name="softActiveProps">激活属性名及更新值，默认:IsActive=1</param>
        /// <returns>更新结果</returns>
        public virtual Task<bool> SwitchActive(TEntity entity, bool isActive,
            object softDeleteProps = null, object softActiveProps = null)
        {
            if (isActive)
            {
                if (softActiveProps == null && Options.SoftActivePropsFactory != null)
                {
                    softActiveProps = Options.SoftActivePropsFactory();
                }

                return Update(entity, softActiveProps);
            }
            else
            {
                if (softDeleteProps == null && Options.SoftDeletePropsFactory != null)
                {
                    softDeleteProps = Options.SoftDeletePropsFactory();
                }

                return Update(entity, softDeleteProps);
            }
        }

        /// <summary>
        /// 根据实体主键ID逻辑删除或激活指定实体
        /// </summary>
        /// <param name="id">实体主键ID</param>
        /// <param name="isActive">是否激活，true:激活，false:逻辑删除</param>
        /// <param name="softDeleteProps">逻辑删除属性名及更新值，默认:IsActive=0</param>
        /// <param name="softActiveProps">激活属性名及更新值，默认:IsActive=1</param>
        /// <returns>更新结果</returns>
        public virtual Task<bool> SwitchActive(TPrimaryKey id, bool isActive,
            object softDeleteProps = null, object softActiveProps = null)
        {
            IPredicate predicate = Configuration.DalImplementor.GetIdPredicate<TEntity>(id);

            if (isActive)
            {
                if (softActiveProps == null && Options.SoftActivePropsFactory != null)
                {
                    softActiveProps = Options.SoftActivePropsFactory();
                }

                return Update(softActiveProps, predicate);
            }
            else
            {
                if (softDeleteProps == null && Options.SoftDeletePropsFactory != null)
                {
                    softDeleteProps = Options.SoftDeletePropsFactory();
                }

                return Update(softDeleteProps, predicate);
            }
        }

        /// <summary>
        /// 根据条件逻辑删除或激活实体
        /// </summary>
        /// <param name="predicate">删除条件</param>
        /// <param name="isActive">是否激活，true:激活，false:逻辑删除</param>
        /// <param name="softDeleteProps">逻辑删除属性名及更新值，默认:IsActive=0</param>
        /// <param name="softActiveProps">激活属性名及更新值，默认:IsActive=1</param>
        /// <returns>更新结果</returns>
        public virtual Task<bool> SwitchActive(object predicate, bool isActive,
            object softDeleteProps = null, object softActiveProps = null)
        {
            if (isActive)
            {
                if (softActiveProps == null && Options.SoftActivePropsFactory != null)
                {
                    softActiveProps = Options.SoftActivePropsFactory();
                }

                return Update(softActiveProps, predicate);
            }
            else
            {
                if (softDeleteProps == null && Options.SoftDeletePropsFactory != null)
                {
                    softDeleteProps = Options.SoftDeletePropsFactory();
                }

                return Update(softDeleteProps, predicate);
            }
        }

        /// <summary>
        /// 根据条件逻辑删除或激活实体
        /// </summary>
        /// <param name="predicate">删除条件</param>
        /// <param name="isActive">是否激活，true:激活，false:逻辑删除</param>
        /// <param name="softDeleteProps">逻辑删除属性名及更新值，默认:IsActive=0</param>
        /// <param name="softActiveProps">激活属性名及更新值，默认:IsActive=1</param>
        /// <returns>更新结果</returns>
        public virtual Task<bool> SwitchActive(Expression<Func<TEntity, bool>> predicate, bool isActive,
            object softDeleteProps = null, object softActiveProps = null)
        {
            if (isActive)
            {
                if (softActiveProps == null && Options.SoftActivePropsFactory != null)
                {
                    softActiveProps = Options.SoftActivePropsFactory();
                }

                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                return Update(softActiveProps, predicateGp);
            }
            else
            {
                if (softDeleteProps == null && Options.SoftDeletePropsFactory != null)
                {
                    softDeleteProps = Options.SoftDeletePropsFactory();
                }

                var predicateGp = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                return Update(softDeleteProps, predicateGp);
            }
        }
    }
}
