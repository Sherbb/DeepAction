using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
[System.Serializable]
public class Ability
{
    public enum AbilityAimMethod
    {
        rayPoint,//cast a ray. send hit point, direction, and entity if hit
        self,//send self point, forward direction, self entity
        noAim,
    }
    
    [System.NonSerialized,OdinSerialize,HideReferenceObjectPicker,HideLabel]
    public DeepBehavior behavior = new DeepBehavior();

}
