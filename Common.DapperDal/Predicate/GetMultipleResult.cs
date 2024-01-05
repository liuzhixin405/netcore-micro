using System.Collections.Generic;
using Dapper;

namespace DapperDal.Predicate
{
    /// <summary>
    /// 多结果集读取器接口
    /// </summary>
    public interface IMultipleResultReader
    {
        /// <summary>
        /// 获取结果集
        /// </summary>
        /// <typeparam name="T">结果集实体类型</typeparam>
        /// <returns>实体集合</returns>
        IEnumerable<T> Read<T>();
    }

    /// <summary>
    /// 多结果集批量读取类
    /// </summary>
    public class GridReaderResultReader : IMultipleResultReader
    {
        private readonly SqlMapper.GridReader _reader;

        /// <summary>
        /// 初始化多结果集批量读取类
        /// </summary>
        /// <param name="reader"></param>
        public GridReaderResultReader(SqlMapper.GridReader reader)
        {
            _reader = reader;
        }

        /// <inheritdoc />
        public IEnumerable<T> Read<T>()
        {
            return _reader.Read<T>();
        }
    }

    /// <summary>
    /// 多结果集顺次读取类
    /// </summary>
    public class SequenceReaderResultReader : IMultipleResultReader
    {
        private readonly Queue<SqlMapper.GridReader> _items;

        /// <summary>
        /// 初始化多结果集顺次读取类
        /// </summary>
        /// <param name="items"></param>
        public SequenceReaderResultReader(IEnumerable<SqlMapper.GridReader> items)
        {
            _items = new Queue<SqlMapper.GridReader>(items);
        }

        /// <inheritdoc />
        public IEnumerable<T> Read<T>()
        {
            SqlMapper.GridReader reader = _items.Dequeue();
            return reader.Read<T>();
        }
    }
}