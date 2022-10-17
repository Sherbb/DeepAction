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

        public bool Trigger()
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


            return true;
        }

        public abstract void InitializeBehavior();
        public abstract void DestroyBehavior();

        public DeepBehavior Clone()
        {
            DeepBehavior b = (DeepBehavior)this.MemberwiseClone();
            return b;
        }
    }
}