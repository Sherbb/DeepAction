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
#endif
    public class DeepAttribute
    {
        public float baseValue { get; private set; }
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public float value { get; private set; }

        private bool clamp;
        public Vector2 minMax { get; private set; }

        private List<DeepAttributeModifier> modifiers;
        [HideInInspector]
        public Action<float> onValueChanged;

        public DeepAttribute()
        {
            this.baseValue = 0f;
            clamp = false;
            modifiers = new List<DeepAttributeModifier>();
            UpdateValue();
        }
        public DeepAttribute(float baseValue)
        {
            this.baseValue = baseValue;
            clamp = false;
            modifiers = new List<DeepAttributeModifier>();
            UpdateValue();
        }
        public DeepAttribute(float baseValue, Vector2 minMax)
        {
            this.baseValue = baseValue;
            this.minMax = minMax;
            clamp = true;
            modifiers = new List<DeepAttributeModifier>();
            UpdateValue();
        }

        public void SetBaseValue(float b)
        {
            baseValue = b;
            UpdateValue();
        }

        public DeepAttributeModifier AddModifier(DeepAttributeModifier modifier)
        {
            modifiers.Add(modifier);
            modifier.onUpdate += UpdateValue;
            UpdateValue();
            return modifier;
        }

        public bool RemoveModifer(DeepAttributeModifier modifier)
        {
            if (modifiers.Contains(modifier))
            {
                modifiers.Remove(modifier);
                modifier.onUpdate -= UpdateValue;
                UpdateValue();
                return true;
            }
            return false;
        }

        public DeepAttribute Clone()
        {
            DeepAttribute newA = (DeepAttribute)this.MemberwiseClone();
            return newA;
        }

        //todo only update once per frame (normally)
        public void UpdateValue()
        {
            float oldValue = value;
            float f = baseValue;

            foreach (DeepAttributeModifier mod in modifiers)
            {
                f += mod.baseAdd;
            }

            float newBase = f;

            foreach (DeepAttributeModifier mod in modifiers)
            {
                f *= (1f + mod.multiplier);
            }

            foreach (DeepAttributeModifier mod in modifiers)
            {
                f += mod.postAdd;
            }

            if (clamp)
            {
                value = Mathf.Clamp(f, minMax.x, minMax.y);
            }
            else
            {
                value = f;
            }

            if (oldValue != value)
            {
                onValueChanged?.Invoke(value);
            }
        }
    }

    public class DeepAttributeModifier
    {
        public DeepEntity owner;

        //BaseValue + baseAdd * multiplier + postAdd
        public float baseAdd { get; private set; }
        public float multiplier { get; private set; }
        public float postAdd { get; private set; }

        public System.Action onUpdate;

        public DeepAttributeModifier()
        {
            this.baseAdd = 0f;
            this.multiplier = 0f;
            this.postAdd = 0f;
        }
        public DeepAttributeModifier(ModValues mod)
        {
            this.baseAdd = mod.baseAdd;
            this.multiplier = mod.multiplier;
            this.postAdd = mod.postAdd;
        }
        /// <summary>
        /// BaseValue + baseAdd * multiplier + postAdd
        /// </summary>
        public DeepAttributeModifier(float baseAdd = 0f, float multiplier = 0f, float postAdd = 0f)
        {
            this.baseAdd = baseAdd;
            this.postAdd = postAdd;
            this.multiplier = multiplier;
        }

        public DeepAttributeModifier(DeepAttributeModifier other)
        {
            this.baseAdd = other.baseAdd;
            this.postAdd = other.postAdd;
            this.multiplier = other.multiplier;
            this.owner = other.owner;
        }

        public void UpdateModifier(float baseAdd, float multiplier, float postAdd)
        {
            this.baseAdd = baseAdd;
            this.multiplier = multiplier;
            this.postAdd = postAdd;
            onUpdate?.Invoke();//if this mod is on an attribute it will cause the attribute to recalculate
        }
    }

    public struct ModValues
    {
        public float baseAdd { get; private set; }
        public float multiplier { get; private set; }
        public float postAdd { get; private set; }

        public ModValues(float baseAdd, float multiplier, float postAdd)
        {
            this.baseAdd = baseAdd;
            this.multiplier = multiplier;
            this.postAdd = postAdd;
        }
    }
}
