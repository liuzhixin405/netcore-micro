using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDb
{
    public class MongoDbService
    {
        private readonly MongoDbOptions _options;
        public MongoDbService(IOptions<MongoDbOptions> options)
        {
            _options = options.Value ?? new MongoDbOptions();
        }

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public async Task<bool> InsertOneAsync<T>(string table, T entity) where T : class, new()
        {
            try
            {
                var client = InitTable<T>(table);
                await client.InsertOneAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 异步批量插入
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="entities">实体集合</param>
        /// <returns></returns>
        public async Task<bool> InsertManyAsync<T>(string table, List<T> entities) where T : class, new()
        {
            try
            {
                var client = InitTable<T>(table);
                await client.InsertManyAsync(entities);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 根据更新条件更新一条记录
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="update">更新</param>
        /// <param name="options">更新选项</param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateOneAsync<T>(string table, FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null) where T : class, new()
        {
            try
            {
                var client = InitTable<T>(table);
                return await client.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 异步修改一条数据
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="entity">添加的实体</param> 
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateAsync<T>(string table, T entity, string id) where T : class, new()
        {
            try
            {
                var client = InitTable<T>(table);
                //修改条件
                var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                //要修改的字段
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (item.Name.ToLower() == "id") continue;
                    list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(entity)));
                }
                var updatefilter = Builders<T>.Update.Combine(list);
                return await client.UpdateOneAsync(filter, updatefilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 异步批量修改数据
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="dic">要修改的字段</param>
        /// <param name="filter">修改条件</param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateManayAsync<T>(string table, Dictionary<string, string> dic, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = InitTable<T>(table);
                T t = new T();
                //要修改的字段
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (!dic.ContainsKey(item.Name)) continue;
                    var value = dic[item.Name];
                    list.Add(Builders<T>.Update.Set(item.Name, value));
                }
                var updatefilter = Builders<T>.Update.Combine(list);
                return await client.UpdateManyAsync(filter, updatefilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 异步删除一条数据
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="id">_id</param>
        /// <param name="useObjeectId">是否使用objectid对象：默认使用</param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteAsync<T>(string table, string id, bool useObjeectId = true) where T : class, new()
        {
            try
            {
                var client = InitTable<T>(table);
                //修改条件
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", id);
                if (useObjeectId)
                    filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                return await client.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 异步删除多条数据
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="filter">删除的条件</param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteManyAsync<T>(string table, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = InitTable<T>(table);
                return await client.DeleteManyAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 根据条件获取总数
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public long CountAsync<T>(string table, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = InitTable<T>(table);
                return client.CountDocuments(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 异步根据id查询一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">表名</param>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<T> FindOneAsync<T>(string table, string id, string[] field = null) where T : class, new()
        {
            try
            {
                var client = InitTable<T>(table);
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    return await client.Find(filter).FirstOrDefaultAsync();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                return await client.Find(filter).Project<T>(projection).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 异步查询集合
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="filter">查询条件</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public async Task<List<T>> FindListAsync<T>(string table, FilterDefinition<T> filter, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var client = InitTable<T>(table);
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return await client.Find(filter).ToListAsync();
                    return await client.Find(filter).Sort(sort).ToListAsync();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                if (sort == null) return await client.Find(filter).Project<T>(projection).ToListAsync();
                //排序查询
                return await client.Find(filter).Sort(sort).Project<T>(projection).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 异步分页查询集合
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="filter">查询条件</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public async Task<List<T>> FindListByPageAsync<T>(string table, FilterDefinition<T> filter, int pageIndex, int pageSize, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var client = InitTable<T>(table);
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return await client.Find(filter).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
                    //进行排序
                    return await client.Find(filter).Sort(sort).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                //不排序
                if (sort == null) return await client.Find(filter).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();

                //排序查询
                return await client.Find(filter).Sort(sort).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取MongoDb实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IMongoCollection<T> InitTable<T>(string tableName) where T : class, new()
        {
            var client = new MongoClient(_options.Connection);
            return client.GetDatabase(_options.DataBase).GetCollection<T>(tableName);
        }
    }
}
