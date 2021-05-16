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

        [SerializeField, HideLabel, HorizontalGroup("hoz"), InlineButton("M"), InlineButton("C")]
        private float baseValue;
        [HideLabel, ShowInInspector, ReadOnly, HorizontalGroup("hoz")]
        private float value;

        [OnValueChanged("UpdateReadOnlyPreview")]
        [SerializeField, HideInInspector]
        private bool clamp;
        private bool showModifiers;//non essential

        [SerializeField, ShowIf("clamp"), HorizontalGroup("hoz2"), LabelWidth(70)]
        private float minValue;
        [SerializeField, ShowIf("clamp"), HorizontalGroup("hoz2"), LabelWidth(70)]
        private float maxValue = 100f;

        [BoxGroup("Active Modifiers"), ShowIf("showModifiers"), ShowInInspector, ReadOnly]
        private float add = 0f;

        [BoxGroup("Active Modifiers"), ShowIf("showModifiers"), ShowInInspector, ReadOnly]
        private List<float> posMultipliers = new List<float>();//multipliers are clamped to a max of 100%
        [BoxGroup("Active Modifiers"), ShowIf("showModifiers"), ShowInInspector, ReadOnly]
        private List<float> negMultipliers = new List<float>();

        public void AddMultplier(float f)
        {
            if (f > 0)
            {
                posMultipliers.Add(f);
            }
            else if (f < 0)
            {
                negMultipliers.Add(f);
            }

            UpdateValue();
        }

        public void RemoveMultiplier(float f)
        {
            if (f > 0)
            {
                if (!posMultipliers.Remove(f))
                {
                    Debug.LogError("Tried to remove a multplier from an attribute that it did not have.");
                }
            }
            else if (f < 0)
            {
                if (!negMultipliers.Remove(f))
                {
                    Debug.LogError("Tried to remove a multplier from an attribute that it did not have.");
                }
            }


            UpdateValue();
        }

        public void AddValueAdd(float f)
        {
            add += f;
            UpdateValue();
        }

        public void RemoveValueAdd(float f)
        {
            add -= f;
            UpdateValue();
        }

        public float GetValue()
        {
            UpdateValue();//need to call UpdateValue() when the object is created, then move this out.
            return value;
        }

        //I am pretty sure we should never need this.
        public float UpdateAndGetValue()
        {
            UpdateValue();
            return value;
        }

        public void UpdateValue()
        {
            float f = baseValue;

            float m = 1f;//m = multiplier


            if (posMultipliers == null)
            {
                posMultipliers = new List<float>();
            }
            if (negMultipliers == null)
            {
                negMultipliers = new List<float>();
            }

            foreach (float v in posMultipliers)
            {
                m *= (1f - Mathf.Clamp(v, 0f, 1f));
                //m *= 1f + v;//exponential ish. The stack get multiplied, dependent on order
                //f += ((baseValue * v) - baseValue);//additive
            }

            m = 1f - m;

            float nm = 1f;//negative multiplier;

            foreach (float v in negMultipliers)
            {
                nm *= (1f - (-Mathf.Clamp(v, -1f, 0f)));
            }

            m -= (1f - nm);

            f *= (1f + m);
            //multipliers are now applied

            f += add;

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
}