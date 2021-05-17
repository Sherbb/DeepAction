using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace DeepAction
{
    [HideReferenceObjectPicker]
    [InlineProperty]
    public class DeepResource
    {
        [OnInspectorInit("@showSettings = false")]
        [OnInspectorGUI("UpdateValues")]

        //Used for stuff like HP. Has special scalings.
        [ProgressBar(0f, "currentMaxValue",ColorGetter ="barColor")]
        [HideLabel,InlineButton("OpenSettings","Settings")]
        public float value = 1f;

        private bool showSettings;


        [ShowIf("showSettings")]
        [BoxGroup("Max"),Delayed,MinValue(0f)]
        public float baseMax = 1f;
        [ShowIf("showSettings")]
        [BoxGroup("Max"),ShowInInspector,ReadOnly,SerializeField]
        private float currentMaxValue;


        [ShowIf("showSettings")]
        [SuffixLabel("num/s",true)][BoxGroup("Regen"),OnValueChanged("UpdateValues")]
        public float baseFlatRegen; // 1 = +1 per second
        [ShowIf("showSettings")]
        [SuffixLabel("%/s",true)][BoxGroup("Regen"),OnValueChanged("UpdateValues")]
        public float basePercentRegen; // .1 = +1% per second
        [ShowIf("showSettings")]
        [SuffixLabel("num/s",true)][BoxGroup("Regen"),ReadOnly]
        public float currentRegen;//

        [ShowIf("showSettings")]
        [FoldoutGroup("Attribute Affectors"),OnValueChanged("UpdateValues")]
        public Dictionary<D_Attribute,float>attributeMaxValueAdd = new Dictionary<D_Attribute, float>();
        [ShowIf("showSettings")]
        [FoldoutGroup("Attribute Affectors"),OnValueChanged("UpdateValues")]
        public Dictionary<D_Attribute,float>attributeMaxValueMultiply = new Dictionary<D_Attribute, float>();
        [ShowIf("showSettings"),OnValueChanged("UpdateValues")]
        [FoldoutGroup("Attribute Affectors")]
        public Dictionary<D_Attribute,float>attributeFlatRegen = new Dictionary<D_Attribute, float>();
        [ShowIf("showSettings"),OnValueChanged("UpdateValues")]
        [FoldoutGroup("Attribute Affectors")]
        public Dictionary<D_Attribute,float>attributePercentRegen = new Dictionary<D_Attribute, float>();

        [HideInInspector]//...
        [FoldoutGroup("Attribute Affectors"),ReadOnly,Tooltip("This SHOULD be autmotically linked for you in all casses. If you are currently in a preset you can ignore the error.")]
        public DeepEntity parentEntity;

        [ShowIf("showSettings")]
        [BoxGroup("Visual")]
        public Color barColor = Color.white;


        //temp variables
        private float ratio;
        private float originalBase;

        #region InspectorStuff
        private void OpenSettings()
        {
            showSettings = !showSettings;
        }
        #endregion

        /// <summary>
        /// Update the resource values and apply regen.
        /// </summary>
        /// <returns>returns the current value after the update</returns>
        public float Tick()
        {
            UpdateValues();
            return value = Mathf.Clamp(value + currentRegen * Time.deltaTime,0f,currentMaxValue);
        }

        //Currently this only gets called ONGUI, and on Tick()
        public void UpdateValues()
        {
            UpdateMaxValue();//should prolly inline these
            UpdateRegenValue();
        }

        public void UpdateMaxValue()
        {
            ratio = value/currentMaxValue;
            currentMaxValue = baseMax;

            originalBase = currentMaxValue;

            if (parentEntity != null)
            {
                foreach(D_Attribute key in attributeMaxValueAdd.Keys)
                {
                    currentMaxValue += attributeMaxValueAdd[key]*parentEntity.GetAttributeValue(key);
                }

                foreach(D_Attribute key in attributeMaxValueMultiply.Keys)
                {
                    currentMaxValue += originalBase * (attributeMaxValueMultiply[key]*parentEntity.GetAttributeValue(key));
                }
            }

            currentMaxValue = Mathf.Clamp(currentMaxValue,0f,Mathf.Infinity);
            value = Mathf.Clamp(ratio * currentMaxValue,0f,currentMaxValue);
        }

        //you want to update regen AFTER maxValue
        public void UpdateRegenValue()
        {
            currentRegen = baseFlatRegen;
            currentRegen += basePercentRegen * currentMaxValue;

            if (parentEntity != null)
            {
                foreach(D_Attribute key in attributeFlatRegen.Keys)
                {
                    currentRegen += attributeFlatRegen[key]*parentEntity.GetAttributeValue(key);
                }

                foreach(D_Attribute key in attributePercentRegen.Keys)
                {
                    currentRegen += currentMaxValue * (attributePercentRegen[key]*parentEntity.GetAttributeValue(key));
                }
            }

        }

        public float GetValue(bool update = false)
        {
            if (update)
            {
                UpdateValues();
            }
            return value;
        }

        public bool TryToConsume(float f, bool update = false)
        {
            if (update)
            {
                UpdateValues();
            }


            if (value >= f)
            {
                value -= f;
                return true;
            }
            else
            {
                return false;
            }
        }

        public DeepResource Clone()
        {
            DeepResource newR = (DeepResource)this.MemberwiseClone();
            return newR;
        }

    }
}