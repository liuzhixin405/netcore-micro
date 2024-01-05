using System.Linq.Expressions;
using System.Reflection;

namespace RepositoryComponent.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<TEntity> GetQueryable<TEntity>(this IQueryable<TEntity> query,Dictionary<string,object> fields) where TEntity : class
        {
            foreach (var field in fields)
            {
                query = query.GetQueryable(field.Key, field.Value);
            }
            return query;
        }


        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query, Dictionary<string, bool> fields) where TEntity : class
        {
            var index = 0;
            foreach (var field in fields)
            {
                if (index == 0)
                    query = query.OrderBy(field.Key, field.Value);
                else
                    query = query.ThenBy(field.Key, field.Value);
                index++;
            }

            return query;
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query,string field,bool desc) where TEntity : class
        {
            var propertyInfo = GetPropertyInfo(typeof(TEntity), field);
            var orderExpression = GetOrderExpression(typeof(TEntity), propertyInfo);
            if(desc)
            {
                var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name.Equals("OrderByDescending") && m.GetParameters().Length == 2);
                var genericMethod = method!.MakeGenericMethod(typeof(TEntity), propertyInfo.PropertyType);
                return (genericMethod.Invoke(null, new object[] { query, orderExpression }) as IQueryable<TEntity>)!;
            }
            else
            {
                var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name.Equals("OrderBy") && m.GetParameters().Length == 2);
                var genericMethod = method!.MakeGenericMethod(typeof(TEntity), propertyInfo.PropertyType);
                return (genericMethod.Invoke(null, new object[] { query, orderExpression }) as IQueryable<TEntity>)!;
            }
        }

        private static IQueryable<T> ThenBy<T>(this IQueryable<T> query,string filed,bool desc) where T : class
        {
            var propertyInfo = GetPropertyInfo(typeof(T), filed);
            var orderExpression = GetOrderExpression(typeof(T), propertyInfo);
            if (desc)
            {
                var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name.Equals("ThenByDescending") && m.GetParameters().Length == 2);
                var genericMethod = method!.MakeGenericMethod(typeof(T), propertyInfo.PropertyType);
                return (genericMethod.Invoke(null, new object[] { query, orderExpression }) as IQueryable<T>)!;
            }
            else
            {
                var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name.Equals("ThenBy") && m.GetParameters().Length == 2);
                var genericMethod = method!.MakeGenericMethod(typeof(T), propertyInfo.PropertyType);
                return (genericMethod.Invoke(null, new object[] { query, orderExpression }) as IQueryable<T>)!;
            }
        }

        private static IQueryable<TEntity> GetQueryable<TEntity>(this IQueryable<TEntity> query, string field, object value) where TEntity:class
        {
            Type type = typeof(TEntity);
            var parameter = Expression.Parameter(type, "entity");

            PropertyInfo property = type.GetProperty(field)!;
            Expression expProperty= Expression.Property(parameter, property.Name);

            Expression<Func<object>> valueLamda = () => value;
            Expression expValue = Expression.Convert(valueLamda.Body, property.PropertyType);
            Expression expression = Expression.Equal(expProperty, expValue);
            Expression<Func<TEntity, bool>> filter = (Expression<Func<TEntity,bool>>)Expression.Lambda(expression, parameter);
            return query.Where(filter);
        }

        private static PropertyInfo GetPropertyInfo(Type entityType,string field)=>
            entityType.GetProperties().FirstOrDefault(p => p.Name.Equals(field,StringComparison.OrdinalIgnoreCase))!;

        private static LambdaExpression GetOrderExpression(Type entityType,PropertyInfo propertyInfo)
        {
            var parameterExpression = Expression.Parameter(entityType);
            var fieldExpression = Expression.PropertyOrField(parameterExpression, propertyInfo.Name);
            return Expression.Lambda(fieldExpression, parameterExpression);
        }
    }



}
