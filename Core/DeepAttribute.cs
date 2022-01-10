using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace DeepAction
{
    [HideReferenceObjectPicker]
    public class DeepAttribute
    {
        [OnInspectorGUI("UpdateValue")]
        [OnInspectorInit("@showModifiers = false")]
        //

        [SerializeField, HideLabel,SuffixLabel("Base Value",true), HorizontalGroup("hoz"), InlineButton("M"), InlineButton("C")]
        private float baseValue;
        [HideLabel, ShowInInspector,SuffixLabel("Final Value",true), ReadOnly, HorizontalGroup("hoz")]
        private float value;

        [OnValueChanged("UpdateReadOnlyPreview")]
        [SerializeField, HideInInspector]
        private bool clamp;/// 
        private bool showModifiers;//non essential

        [SerializeField, ShowIf("clamp"), HorizontalGroup("hoz2"), LabelWidth(70)]
        private float minValue;
        [SerializeField, ShowIf("clamp"), HorizontalGroup("hoz2"), LabelWidth(70)]
        private float maxValue = 100f;

        [BoxGroup("Active Modifiers"), ShowIf("showModifiers"), ShowInInspector,ReadOnly]
        private List<DeepAttributeModifier> modifiers = new List<DeepAttributeModifier>();

        [BoxGroup("Active Modifiers"), ShowIf("showModifiers")][Tooltip("If true the HIGHEST override will be chosen.")]
        public bool overrideToMax;//if true we will pick the largest override value.

        [BoxGroup("Active Modifiers"), ShowIf("showModifiers"), ShowInInspector,ReadOnly]
        private List<float> overrides = new List<float>();

        public DeepAttributeModifier AddModifier(DeepAttributeModifier modifier)
        {
            if (modifiers == null)
            {
                modifiers = new List<DeepAttributeModifier>();
            }
            modifiers.Add(modifier);
            return modifier;
        }

        public bool RemoveModifer(DeepAttributeModifier modifier)
        {
            if (modifiers.Contains(modifier))
            {
                modifiers.Remove(modifier);
                return true;
            }
            return false;
        }

        public void AddOverride(float value)
        {
            overrides.Add(value);
        }

        public bool RemoveOverride(float value)
        {
            if (overrides.Contains(value))
            {
                overrides.Remove(value);
                return true;
            }
            return false;
        }

        public DeepAttribute Clone()
        {
            DeepAttribute newA = (DeepAttribute)this.MemberwiseClone();
            return newA;
        }

        public float GetValue()
        {
            UpdateValue();
            return value;
        }

        public void UpdateValue()
        {
            //baseValue + baseAdd * baseMultiply + postAdd

            if (overrides == null)
            {
                overrides = new List<float>();
            }

            if (modifiers == null)
            {
                modifiers = new List<DeepAttributeModifier>();
            }

            float f = baseValue;

            //override
            if (overrides.Count > 0)
            {
                f = overrides[0];

                if (overrideToMax)
                {
                    foreach(float ovr in overrides)
                    {
                        if (ovr > f)
                        {
                            f = ovr;
                        }
                    }
                }
                else
                {
                    foreach(float ovr in overrides)
                    {
                        if (ovr < f)
                        {
                            f = ovr;
                        }
                    }
                }
                value = f;
                return;
            }

            foreach(DeepAttributeModifier mod in modifiers)
            {
                f += mod.baseAdd;
            }

            float newBase = f;

            foreach(DeepAttributeModifier mod in modifiers)
            {
                f += (newBase * mod.multiplier);
            }

            foreach(DeepAttributeModifier mod in modifiers)
            {
                f += mod.postAdd;
            }

            if (clamp)
            {
                value = Mathf.Clamp(f, minValue, maxValue);
            }
            else
            {
                value = f;
            }
        }

        #region InspectorStuff
        private void C()
        {
            clamp = !clamp;
        }
        private void M()
        {
            showModifiers = !showModifiers;
        }
        #endregion
    }

    [HideReferenceObjectPicker,InlineProperty]
    public class DeepAttributeModifier
    {
        //BaseValue + baseAdd * multiplier + postAdd

        [SuffixLabel("Source",true),HideLabel]
        public string source = "Unknown";//exists just to help you keep track of stuff in inspectors.

        [HorizontalGroup("hoz"),SuffixLabel("BaseAdd",true),HideLabel]
        public float baseAdd;
        [HorizontalGroup("hoz"),SuffixLabel("Multiplier",true),HideLabel]
        public float multiplier;
        [HorizontalGroup("hoz"),SuffixLabel("Post Add",true),HideLabel]
        public float postAdd;

        public DeepAttributeModifier()
        {
            this.baseAdd = 0f;
            this.multiplier = 0f;
            this.postAdd = 0f;
        }
        public DeepAttributeModifier(float baseAdd,float multiplier,float postAdd)
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

    }
}