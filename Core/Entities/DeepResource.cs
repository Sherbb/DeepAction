using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
namespace DeepAction
{
#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
    [InlineProperty]
#endif
    /// <summary>
    /// A resource is an int with a max value
    /// - the max value can be modified by behaviors. Mods should only exist while a behvior is on an entity.
    /// - behaviors can freely regen and consume the resource 
    /// </summary>
    public class DeepResource
    {
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public int value { get; private set; }

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public int baseMax { get; private set; }
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public int currentMax { get; private set; }

        [HideInInspector]
        public Action<int> onConsumeVal;
        [HideInInspector]
        public Action onConsume;//sometimes you dont want the int....
        [HideInInspector]
        public Action<int> onRegenVal;
        [HideInInspector]
        public Action onRegen;
        [HideInInspector]
        public Action onDeplete;
        [HideInInspector]
        public Action onFill;

        private List<DeepResourceModifier> modifiers;

        public bool isFull => value == currentMax;
        public bool isEmpty => value == 0;

        public DeepResource(int baseMax, int baseValue)
        {
            this.value = baseValue;
            this.baseMax = baseMax;
            modifiers = new List<DeepResourceModifier>();
            UpdateMaxValue();
        }

        public int Regen(int r)
        {
            if (value == currentMax)
            {
                return value;
            }
            int newV = Mathf.Clamp(value + r, 0, currentMax);
            int regened = newV - value;
            value = newV;
            onRegenVal?.Invoke(regened);
            onRegen?.Invoke();
            if (value == currentMax)
            {
                onFill?.Invoke();
            }
            return value;
        }

        public bool TryToConsume(int c)
        {
            if (value >= c)
            {
                Consume(c);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Subtract the value from the resource
        /// </summary>
        /// <returns>Returns the remainder</returns>
        public int Consume(int c)
        {
            if (value == 0) return c;
            int newV = Mathf.Clamp(value - c, 0, currentMax);
            int consumed = value - newV;
            value = newV;
            onConsumeVal?.Invoke(consumed);
            onConsume?.Invoke();
            if (value <= 0)
            {
                onDeplete?.Invoke();
            }
            return c - consumed;
        }

        /// <summary>
        /// You probably don't want to use this outside of initializing a deepEntity. Use regen and consume instead.
        /// </summary>
        public int SetValue(int v)
        {
            value = Mathf.Clamp(v, 0, currentMax);
            return value;
        }

        public int SetValueWithRatio(float r)
        {
            value = (int)(currentMax * Mathf.Clamp(r, 0f, 1f));
            return value;
        }

        public void AddModifier(DeepResourceModifier mod)
        {
            if (modifiers.Contains(mod))
            {
                Debug.LogError("A resource mod was added multiple times to the same resource");
                return;
            }
            modifiers.Add(mod);
            mod.onUpdate += UpdateMaxValue;
            UpdateMaxValue();
        }

        public void RemoveModifier(DeepResourceModifier mod)
        {
            if (modifiers.Contains(mod))
            {
                modifiers.Remove(mod);
                mod.onUpdate -= UpdateMaxValue;
                UpdateMaxValue();
            }
        }

        private void UpdateMaxValue()
        {
            int b = baseMax;

            foreach (DeepResourceModifier m in modifiers)
            {
                b += m.maxModify;
            }
            b = Mathf.Max(b, 0);

            currentMax = b;
            value = Mathf.Clamp(value, 0, b);
        }

        public DeepResource Clone()
        {
            DeepResource newR = (DeepResource)this.MemberwiseClone();
            return newR;
        }
#if ODIN_INSPECTOR
        [HideReferenceObjectPicker]
#endif
        public class DeepResourceModifier
        {
            public int maxModify { get; private set; }
            public Action onUpdate;

            public DeepResourceModifier(int maxModify)
            {
                this.maxModify = maxModify;
            }

            public void UpdateModifier(int newMaxModify)
            {
                this.maxModify = newMaxModify;
                onUpdate?.Invoke();
            }
        }
    }
}