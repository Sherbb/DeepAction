using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace DeepAction
{
    [DoNotDrawAsReference]
    public abstract class DeepBehavior
    {
        public Dictionary<D_Resource, float> resourcesToTrigger = new Dictionary<D_Resource, float>();

        [HideInInspector]
        public DeepEntity parent;

        //todo
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

    //lets us have an action with a ref. MOVE THIS
    //
    //for example we can give OnTakeDamage a REF float allowing behaviors to modify incoming damage before it is applied.
    public delegate void ActionRef<T>(ref T item);
}