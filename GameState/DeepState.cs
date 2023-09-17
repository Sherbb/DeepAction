using System;
using System.Collections;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace DeepAction
{
    [Serializable]
#if ODIN_INSPECTOR
    [ShowInInspector, InlineProperty, HideReferenceObjectPicker]
#endif
    public class DeepState<T>
    {
#if ODIN_INSPECTOR
        [ShowInInspector, HideLabel]
#endif
        //if you use json need to rename the json property here because "value" can be interperated weirdly in json collections
        public T value { get; private set; }

        [HideInInspector]
        public Action<T> onValueChanged;

        public DeepState(T baseValue)
        {
            value = baseValue;
        }

        public DeepState()
        {
            value = default;
        }

        public T SetValue(T newValue)
        {
            value = newValue;
            onValueChanged?.Invoke(value);
            return value;
        }
    }

    [Serializable]
    public class DeepStateList<T> : IEnumerable<T>
    {
#if ODIN_INSPECTOR
        [ShowInInspector, HideLabel]
#endif
        public List<T> list { get; private set; }

        /// <summary>called when an objects is added/removed</summary>
        [HideInInspector]
        public Action onCollectionChanged;

        public DeepStateList()
        {
            list = new List<T>();
        }

        public T this[int index]
        {
            get
            {
                return list[index];
            }
        }

        public void Add(T entry)
        {
            list.Add(entry);
            onCollectionChanged?.Invoke();
        }

        public bool Remove(T entry)
        {
            if (list.Remove(entry))
            {
                onCollectionChanged?.Invoke();
                return true;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T v in list)
            {
                yield return v;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    //todo dont think we will use this....
    public class DeepStateDictionary<TKey, TValue>
    {
        public Dictionary<TKey, TValue> dictionary { get; private set; }

        public Action<Dictionary<TKey, TValue>> onCollectionChanged;

        public static implicit operator Dictionary<TKey, TValue>(DeepStateDictionary<TKey, TValue> d) => d.dictionary;

        public bool Add(TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
                onCollectionChanged?.Invoke(dictionary);
                return true;
            }
            return false;
        }

        //incomplete
        public bool Remove(TKey key)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                onCollectionChanged?.Invoke(dictionary);
                return true;
            }
            return false;
        }
    }
}