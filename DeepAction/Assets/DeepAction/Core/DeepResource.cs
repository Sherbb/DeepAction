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
        //Used for stuff like HP. Has special scalings.
        [ProgressBar(0f, 100f)]
        [HideLabel]
        public float value;
        //public float maxValue;
        //public Color color;

        public float GetValue()
        {
            return value;
        }

        public bool TryToConsume(float f)
        {
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