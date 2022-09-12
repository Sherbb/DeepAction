using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeepAction
{
    [Serializable, ShowInInspector, InlineProperty, HideReferenceObjectPicker]
    public class DeepState<T>
    {
        //JsonProperty here is required for DeepStateSet<> etc to deserialize properly... idk why
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
    //todo this needs to implement enumerable or something so that we can use []
    public class DeepStateList<T>
    {
        [ShowInInspector, HideLabel]
        public List<T> list { get; private set; }

        /// <summary>
        /// called when an objects is added/removed
        /// </summary>
        [HideInInspector, JsonIgnore]
        public Action<List<T>> onCollectionChanged;

        public DeepStateList()
        {
            list = new List<T>();
        }

        public void Add(T entry)
        {
            list.Add(entry);
            onCollectionChanged?.Invoke(list);
        }

        public bool Remove(T entry)
        {
            if (list.Remove(entry))
            {
                onCollectionChanged?.Invoke(list);
                return true;
            }
            return false;
        }
    }
/*
    [Serializable]
    public class DeepStateList<T>
    {
        [ShowInInspector, HideLabel]
        public List<DeepState<T>> collection { get; private set; }

        /// <summary>
        /// called when an objects is added/removed
        /// </summary>
        [HideInInspector, JsonIgnore]
        public Action<List<DeepState<T>>> onCollectionChanged;

        /// <summary>
        /// when a member of the set is changed this is triggered
        /// </summary>
        [HideInInspector, JsonIgnore]
        public Action onValueChanged;

        public DeepStateList()
        {
            collection = new List<DeepState<T>>();
        }

        public void Add(DeepState<T> entry)
        {
            collection.Add(entry);
            entry.onValueChanged += ValueChanged;
            onCollectionChanged?.Invoke(collection);
        }

        public bool Remove(DeepState<T> entry)
        {
            if (collection.Remove(entry))
            {
                entry.onValueChanged -= ValueChanged;
                onCollectionChanged?.Invoke(collection);
                return true;
            }
            return false;
        }

        private void ValueChanged(T value)
        {
            onValueChanged?.Invoke();
        }
    }
*/

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