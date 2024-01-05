using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DapperDal.Implementor;
using DapperDal.Mapper;
using DapperDal.Sql;

namespace DapperDal
{
    /// <summary>
    /// 数据访问配置接口
    /// </summary>
    public interface IDalConfiguration
    {
         /// <summary>
         /// 默认实体映射类型
         /// </summary>
         Type DefaultMapper { get; set; }

        /// <summary>
        /// 实体映射类型检索程序集
        /// </summary>
        IList<Assembly> MappingAssemblies { get; set; }

         /// <summary>
         /// SQL方言实例
         /// </summary>
         ISqlDialect Dialect { get; set; }

         /// <summary>
         /// 数据访问实现实例
         /// </summary>
         IDalImplementor DalImplementor { get; set; }

        /// <summary>
        /// 生成SQL时，是否添加 WITH (NOLOCK)
        /// </summary>
         bool Nolock { get; set; }

        /// <summary>
        /// SQL输出方法
        /// </summary>
         Action<string> OutputSql { get; set; }

        /// <summary>
        /// 实体集合返回前是否要缓冲（ToList）
        /// </summary>
         bool Buffered { get; set; }

        /// <summary>
        /// 设置实体映射类型检索程序集
        /// </summary>
        /// <param name="assemblies">实体映射类型检索程序集</param>
        void SetMappingAssemblies(IList<Assembly> assemblies);

        /// <summary>
        /// 获取实体映射类型
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>实体映射类型</returns>
        IClassMapper GetMap(Type entityType);

        /// <summary>
        /// 获取实体映射类型
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>实体映射类型</returns>
        IClassMapper GetMap<T>() where T : class;

        /// <summary>
        /// 清空实体映射类型缓存
        /// </summary>
        void ClearCache();

        /// <summary>
        /// 获取新的GUID
        /// </summary>
        /// <returns>新的GUID</returns>
        Guid GetNextGuid();
    }

    /// <summary>
    /// 数据访问配置类
    /// </summary>
    public class DalConfiguration : IDalConfiguration
    {
        private readonly ConcurrentDictionary<Type, IClassMapper> _classMaps = new ConcurrentDictionary<Type, IClassMapper>();

        static DalConfiguration()
        {
            Default = new DalConfiguration();
        }

        /// <summary>
        /// 初始化数据访问配置
        /// </summary>
        public DalConfiguration()
            : this(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect())
        {
        }


        /// <summary>
        /// 初始化数据访问配置
        /// </summary>
        /// <param name="defaultMapper">默认实体映射类型</param>
        /// <param name="mappingAssemblies">实体映射类型检索程序集</param>
        /// <param name="sqlDialect">SQL方言实例</param>
        public DalConfiguration(Type defaultMapper, IList<Assembly> mappingAssemblies, ISqlDialect sqlDialect)
        {
            DefaultMapper = defaultMapper;
            MappingAssemblies = mappingAssemblies ?? new List<Assembly>();
            Dialect = sqlDialect;
            DalImplementor = new DalImplementor(new SqlGeneratorImpl(this));
        }

        /// <summary>
        /// 全局默认配置
        /// </summary>
        public static IDalConfiguration Default { get; }

        /// <inheritdoc />
        public Type DefaultMapper { get; set; }

        /// <inheritdoc />
        public IList<Assembly> MappingAssemblies { get; set; }

        /// <inheritdoc />
        public ISqlDialect Dialect { get; set; }

        /// <inheritdoc />
        public IDalImplementor DalImplementor { get; set; }

        /// <summary>
        /// 生成SQL时，是否添加 WITH (NOLOCK)
        /// </summary>
        public bool Nolock { get; set; }

        /// <summary>
        /// SQL输出方法
        /// </summary>
        public Action<string> OutputSql { get; set; }

        /// <summary>
        /// 实体集合返回前是否要缓冲（ToList）
        /// </summary>
        public bool Buffered { get; set; }

        /// <inheritdoc />
        public void SetMappingAssemblies(IList<Assembly> assemblies)
        {
            MappingAssemblies = assemblies ?? new List<Assembly>();
            ClearCache();
            DalImplementor = new DalImplementor(new SqlGeneratorImpl(this));
        }

        /// <inheritdoc />
        public IClassMapper GetMap(Type entityType)
        {
            IClassMapper map;
            if (!_classMaps.TryGetValue(entityType, out map))
            {
                Type mapType = GetMapType(entityType);
                if (mapType == null)
                {
                    mapType = DefaultMapper.MakeGenericType(entityType);
                }

                map = Activator.CreateInstance(mapType) as IClassMapper;
                _classMaps[entityType] = map;
            }

            return map;
        }

        /// <inheritdoc />
        public IClassMapper GetMap<T>() where T : class
        {
            return GetMap(typeof(T));
        }

        /// <inheritdoc />
        public void ClearCache()
        {
            _classMaps.Clear();
        }

        /// <inheritdoc />
        public Guid GetNextGuid()
        {
            byte[] b = Guid.NewGuid().ToByteArray();
            DateTime dateTime = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            TimeSpan timeSpan = new TimeSpan(now.Ticks - dateTime.Ticks);
            TimeSpan timeOfDay = now.TimeOfDay;
            byte[] bytes1 = BitConverter.GetBytes(timeSpan.Days);
            byte[] bytes2 = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));
            Array.Reverse(bytes1);
            Array.Reverse(bytes2);
            Array.Copy(bytes1, bytes1.Length - 2, b, b.Length - 6, 2);
            Array.Copy(bytes2, bytes2.Length - 4, b, b.Length - 4, 4);
            return new Guid(b);
        }

        private Type GetMapType(Type entityType)
        {
            Func<Assembly, Type> getType = a =>
            {
                Type[] types = a.GetTypes();
                return (from type in types
                        let interfaceType = type.GetInterface(typeof(IClassMapper<>).FullName)
                        where
                            interfaceType != null &&
                            interfaceType.GetGenericArguments()[0] == entityType
                        select type).SingleOrDefault();
            };

            Type result = getType(entityType.Assembly);
            if (result != null)
            {
                return result;
            }

            foreach (var mappingAssembly in MappingAssemblies)
            {
                result = getType(mappingAssembly);
                if (result != null)
                {
                    return result;
                }
            }

            return getType(entityType.Assembly);
        }
    }
}