using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Newtonsoft.Json;

namespace DeepAction
{
    [Serializable, ShowInInspector, InlineProperty, HideReferenceObjectPicker]
    public class DeepState<T>
    {
        //need to rename the json property here because "value" can be interperated weirdly in json collections
        [ShowInInspector, HideLabel, JsonProperty("State")]
        public T value { get; private set; }

        [HideInInspector, JsonIgnore]
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
        [ShowInInspector, HideLabel]
        public List<T> list { get; private set; }

        /// <summary>called when an objects is added/removed</summary>
        [HideInInspector, JsonIgnore]
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