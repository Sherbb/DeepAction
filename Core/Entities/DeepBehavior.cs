using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace DeepAction
{
    [DoNotDrawAsReference]
    public abstract class DeepBehavior
    {
        public Dictionary<D_Resource, int> resourcesToTrigger = new Dictionary<D_Resource, int>();

        [HideInInspector]
        /// <summary>
        /// The deepEntity that this behavior is on.
        /// </summary>
        public DeepEntity parent;

        //* Flags
        [HideInInspector]
        public bool removeOnDeath { get; private set; }



        //todo, idk what the point of this will be.
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
                    if (parent.resources[key].value < resourcesToTrigger[key])
                    {
                        return false;
                    }
                }
            }

            foreach (D_Resource key in resourcesToTrigger.Keys)
            {
                parent.resources[key].TryToConsume(resourcesToTrigger[key]);
            }

            parent.events.Trigger?.Invoke(point, direction, target);

            return true;
        }

        public abstract void IntitializeBehavior();
        public abstract void DestroyBehavior();

        public DeepBehavior Clone()
        {
            DeepBehavior b = (DeepBehavior)this.MemberwiseClone();
            return b;
        }
    }

    //lets us have an action with a ref. MOVE THIS
    //
    //for example we can give OnTakeDamage a REF float allowing behaviors to modify incoming damage before it is applied.
    public delegate void ActionRef<T>(ref T item);
}