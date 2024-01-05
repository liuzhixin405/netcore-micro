using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpComponent
{
    /// <summary>
    ///  A dictionary in which one key has many associated values.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Multimap<TKey, TValue> : IDictionary<TKey, IList<TValue>>
    {

        #region private fields
        private readonly Dictionary<TKey, IList<TValue>> _dictionary; 
        #endregion

        #region ctor
        public Multimap()
        {
            _dictionary = new Dictionary<TKey, IList<TValue>>();
        }
        public Multimap(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, IList<TValue>>(comparer);
        } 
        #endregion
        public IList<TValue> this[TKey key] { get => _dictionary[key]; set => _dictionary[key] = value; }

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<IList<TValue>> Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            if (value != null)
            {
                if(_dictionary.TryGetValue(key,out var list))
                {
                    list.Add(value);
                }
                else
                {
                    list = new List<TValue> { value };
                    if(!TryAdd(key,list))
                        throw new InvalidOperationException("Could not add value to Multimap.");
                }
            }
        }

       

        public void Add(KeyValuePair<TKey, IList<TValue>> item)
        {
         if(!TryAdd(item.Key,item.Value))
                throw new InvalidOperationException("Could not add values to Multimap.");
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, IList<TValue>> item)
        {
            return _dictionary.ContainsKey(item.Key) && _dictionary.ContainsValue(item.Value);
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, IList<TValue>>[] array, int arrayIndex)
        {
            ((ICollection)_dictionary).CopyTo(array, arrayIndex);
        }
        public void CopyTo(Array array, int arrayIndex)
        {
            ((ICollection)_dictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(TKey key)
        {
            return TryRemove(key, out var _);
        }

        public bool Remove(KeyValuePair<TKey, IList<TValue>> item)
        {
            if (_dictionary.ContainsKey(item.Key))
            {
                _dictionary.Remove(item.Key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out IList<TValue> value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        #region Enumerators

        /// <summary>
        /// To get the enumerator.
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator<KeyValuePair<TKey, IList<TValue>>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        /// <summary>
        /// To get the enumerator.
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        #endregion Enumerators

        public void Add(TKey key, IList<TValue> value)
        {
            throw new NotImplementedException();
        }

        #region Private Methods
        private bool TryAdd(TKey key, IList<TValue> list)
        {
            try
            {
                _dictionary.Add(key, list);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

        private bool TryRemove(TKey key,out IList<TValue> value)
        {
            _dictionary.TryGetValue(key, out value);
            return _dictionary.Remove(key);
        }
        #endregion
    }
}
