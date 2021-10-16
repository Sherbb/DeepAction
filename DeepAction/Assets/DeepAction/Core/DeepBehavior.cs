using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace DeepAction
{
    [System.Serializable][HideReferenceObjectPicker]
    public abstract class DeepBehavior
    {
        public abstract string behaviorID {get;}
        public Dictionary<D_Resource, float> resourcesToTrigger = new Dictionary<D_Resource, float>();//resources require to TRIGGER the behavior. This is where you put the spell cost. Can be set by actions.

        [HideInInspector]
        public DeepEntity parent;


        public bool Trigger(Vector3 point, Vector3 direction, DeepEntity target)
        {
            foreach (D_Resource key in resourcesToTrigger.Keys)
            {
                if (!parent.resources.ContainsKey(key))
                {
                    Debug.LogError(parent.gameObject.name + "Does not have the resource: " + key);
                }
                else
                {
                    if (parent.resources[key].GetValue() < resourcesToTrigger[key])
                    {
                        return false;
                    }
                }
            }

            foreach (D_Resource key in resourcesToTrigger.Keys)
            {
                parent.resources[key].TryToConsume(resourcesToTrigger[key]);
            }

            parent.events.Trigger?.Invoke(point,direction,target);

            return true;
        }

        public abstract void IntitializeBehavior();
        public abstract void DestroyBehavior();
    }

    //lets us have an action with a ref.
    //
    //for example we can give OnTakeDamage a REF float allowing behaviors to modify incoming damage before it is applied.
    public delegate void ActionRef<T>(ref T item);
}