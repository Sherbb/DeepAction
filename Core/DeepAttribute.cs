using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace DeepAction
{
    [HideReferenceObjectPicker]
    public class DeepAttribute
    {
        public float baseValue { get; private set; }
        public float value { get; private set; }

        private bool clamp;
        public Vector2 minMax { get; private set; }

        private List<DeepAttributeModifier> modifiers;
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
                f += (newBase * mod.multiplier);
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
                onValueChanged(value);
            }
        }
    }

    public class DeepAttributeModifier
    {
        //BaseValue + baseAdd * multiplier + postAdd
        public DeepBehavior source;

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
        public DeepAttributeModifier(float baseAdd, float multiplier, float postAdd)
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
            this.source = other.source;
        }

        public void UpdateModifier(float baseAdd, float multiplier, float postAdd)
        {
            this.baseAdd = baseAdd;
            this.multiplier = multiplier;
            this.postAdd = postAdd;
            onUpdate?.Invoke();//if this mod is on an attribute it will cause the attribute to recalculate
        }
    }
}
