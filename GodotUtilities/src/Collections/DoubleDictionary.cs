using System.Collections;
using System.Collections.Generic;

namespace GodotUtilities.Collections
{
    public class DoubleDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _keyToValue = new Dictionary<TKey, TValue>();
        private Dictionary<TValue, TKey> _valueToKey = new Dictionary<TValue, TKey>();

        public TValue this [TKey key]
        {
            get
            {
                return _keyToValue[key];
            }
            set
            {
                _keyToValue[key] = value;
                _valueToKey[value] = key;
            }
        }

        public TKey this [TValue val]
        {
            get
            {
                return _valueToKey[val];
            }
            set
            {
                _valueToKey[val] = value;
                _keyToValue[value] = val;
            }
        }

        public ICollection<TKey> Keys => _keyToValue.Keys;

        public ICollection<TValue> Values => _valueToKey.Keys;

        public int Count => _keyToValue.Count;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            _keyToValue[key] = value;
            _valueToKey[value] = key;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _keyToValue[item.Key] = item.Value;
            _valueToKey[item.Value] = item.Key;
        }

        public void Clear()
        {
            _keyToValue.Clear();
            _valueToKey.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _keyToValue.ContainsKey(item.Key) && _valueToKey.ContainsKey(item.Value);
        }

        public bool ContainsKey(TKey key)
        {
            return _keyToValue.ContainsKey(key);
        }

        public bool ContainsKey(TValue value)
        {
            return _valueToKey.ContainsKey(value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {

        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _keyToValue.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            if (_keyToValue.ContainsKey(key))
            {
                var val = _keyToValue[key];
                _keyToValue.Remove(key);
                _valueToKey.Remove(val);
                return true;
            }
            return false;
        }

        public bool Remove(TValue value)
        {
            if (_valueToKey.ContainsKey(value))
            {
                var key = _valueToKey[value];
                _valueToKey.Remove(value);
                _keyToValue.Remove(key);
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_keyToValue.ContainsKey(key))
            {
                value = _keyToValue[key];
                return true;
            }
            value = default(TValue);
            return false;
        }

        public bool TryGetValue(TValue value, out TKey key)
        {
            if (_valueToKey.ContainsKey(value))
            {
                key = _valueToKey[value];
                return true;
            }
            key = default(TKey);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _keyToValue.GetEnumerator();
        }
    }
}