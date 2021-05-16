using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace DeepAction
{

    [CreateAssetMenu]
    public class AbilityObject : SerializedScriptableObject
    {
        [System.NonSerialized, OdinSerialize, HideReferenceObjectPicker, HideLabel]
        public Ability ability = new Ability();
    }
}
