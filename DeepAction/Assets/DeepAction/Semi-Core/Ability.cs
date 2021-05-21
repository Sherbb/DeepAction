using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Newtonsoft.Json;

namespace DeepAction
{
    [System.Serializable]
    public class Ability
    {

        //unfinished
        //
        //An ability would have extra information that is only relevant to the player.
        //stuff like aim method, descriptions, icons.

        public enum AbilityAimMethod
        {
        }

        //[System.NonSerialized, OdinSerialize, HideReferenceObjectPicker, HideLabel]
        [InlineProperty,HideLabel]
        public DeepBehavior behavior = new DeepBehavior();

    }
}