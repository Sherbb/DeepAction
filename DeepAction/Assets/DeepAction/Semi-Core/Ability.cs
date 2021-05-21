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
        public string abilityName = "what";
        public float baseAbilityCooldown;
        public enum AbilityAimMethod
        {
            rayPoint,//cast a ray. send hit point, direction, and entity if hit
            self,//send self point, forward direction, self entity
            noAim,
        }

        //[System.NonSerialized, OdinSerialize, HideReferenceObjectPicker, HideLabel]
        [InlineProperty,HideLabel]
        public DeepBehavior behavior = new DeepBehavior();

    }
}