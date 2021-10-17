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
        [ProgressBar(0f, "currentMaxValue", ColorGetter = "barColor")]
        [HideLabel, InlineButton("OpenSettings", "Settings")]
        public float value = 1f;

        private bool showSettings;


        [ShowIf("showSettings")]
        [BoxGroup("Max"), Delayed, MinValue(0f)]
        public float baseMax = 1f;
        [ShowIf("showSettings")]
        [BoxGroup("Max"), ReadOnly, SerializeField]
        private float currentMaxValue;


        [ShowIf("showSettings")]
        [SuffixLabel("num/s", true)]
        [BoxGroup("Regen"), OnValueChanged("UpdateValues")]
        public float baseFlatRegen; // 1 = +1 per second
        [ShowIf("showSettings")]
        [SuffixLabel("%/s", true)]
        [BoxGroup("Regen"), OnValueChanged("UpdateValues")]
        public float basePercentRegen; // .01 = +1% per second
        [ShowIf("showSettings")]
        [SuffixLabel("num/s", true)]
        [BoxGroup("Regen"), ReadOnly]
        public float currentRegen;//

        [ShowIf("showSettings")]
        [FoldoutGroup("Attribute Affectors"), OnValueChanged("UpdateValues")]
        public Dictionary<D_Attribute, float> attributeMaxValueAdd = new Dictionary<D_Attribute, float>();
        [ShowIf("showSettings")]
        [FoldoutGroup("Attribute Affectors"), OnValueChanged("UpdateValues")]
        public Dictionary<D_Attribute, float> attributeMaxValueMultiply = new Dictionary<D_Attribute, float>();
        [ShowIf("showSettings"), OnValueChanged("UpdateValues")]
        [FoldoutGroup("Attribute Affectors")]
        public Dictionary<D_Attribute, float> attributeFlatRegen = new Dictionary<D_Attribute, float>();
        [ShowIf("showSettings"), OnValueChanged("UpdateValues")]
        [FoldoutGroup("Attribute Affectors")]
        public Dictionary<D_Attribute, float> attributePercentRegen = new Dictionary<D_Attribute, float>();

        [HideInInspector]//...
        [FoldoutGroup("Attribute Affectors"), ReadOnly]
        public DeepEntity parentEntity;

        [ShowIf("showSettings")]
        [BoxGroup("Other")]
        public Color barColor = Color.white;
        [ShowIf("showSettings")]
        [BoxGroup("Other")]
        [Tooltip("What % the resources will be filled when an entity is initialized. 1 = full")]
        public float defaultRatio = 1f;


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
            return value = Mathf.Clamp(value + currentRegen * Time.deltaTime, 0f, currentMaxValue);
        }

        public void UpdateValues()
        {
            //* Update MAX VALUE
            //we have to update the max value all the time because regen is bassed on max value
            ratio = value / currentMaxValue;
            currentMaxValue = baseMax;

            originalBase = currentMaxValue;

            if (parentEntity != null)
            {
                foreach (D_Attribute key in attributeMaxValueAdd.Keys)
                {
                    currentMaxValue += attributeMaxValueAdd[key] * parentEntity.GetAttributeValue(key);
                }

                foreach (D_Attribute key in attributeMaxValueMultiply.Keys)
                {
                    currentMaxValue += originalBase * (attributeMaxValueMultiply[key] * parentEntity.GetAttributeValue(key));
                }
            }

            currentMaxValue = Mathf.Clamp(currentMaxValue, 1f, Mathf.Infinity);
            value = Mathf.Clamp(ratio * currentMaxValue, 0f, currentMaxValue);


            //* Update REGEN VALUE
            currentRegen = baseFlatRegen;
            currentRegen += basePercentRegen * currentMaxValue;

            if (parentEntity != null)
            {
                foreach (D_Attribute key in attributeFlatRegen.Keys)
                {
                    currentRegen += attributeFlatRegen[key] * parentEntity.GetAttributeValue(key);
                }

                foreach (D_Attribute key in attributePercentRegen.Keys)
                {
                    currentRegen += currentMaxValue * (attributePercentRegen[key] * parentEntity.GetAttributeValue(key));
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

        public float ConsumeWithRemainder(float f, bool update = false)
        {
            if (update)
            {
                UpdateValues();
            }

            float take = Mathf.Clamp(f, 0f, value);

            value -= take;

            return f - take;
        }

        public float SetValueWithRatio(float r, bool update = false)
        {
            if (update)
            {
                UpdateValues();
            }

            value = currentMaxValue * Mathf.Clamp(r, 0f, 1f);
            return value;
        }

        public DeepResource Clone()
        {
            DeepResource newR = (DeepResource)this.MemberwiseClone();
            return newR;
        }

    }
}