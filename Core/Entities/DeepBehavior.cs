using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace DeepAction
{
    [DoNotDrawAsReference]
    public abstract class DeepBehavior
    {
        /// <summary>The deepEntity that this behavior is on.</summary>
        [HideInInspector]
        public DeepEntity parent;

        //* Flags
        [HideInInspector]
        public virtual bool removeOnDeath { get; private set; }
        public virtual bool canBeCast { get; private set; }

        public virtual Dictionary<D_Resource, int> resourcesToCast { get; private set; } = new Dictionary<D_Resource, int>();

        public virtual void InitializeBehavior() { }
        public virtual void DestroyBehavior() { }
        public virtual void OnCast() { }

        public bool Cast()
        {
            if (!canBeCast)
            {
                Debug.LogError("Cast called on behavior with canBeCast flag false", this.parent.gameObject);
                return false;
            }

            foreach (D_Resource key in resourcesToCast.Keys)
            {
                if (parent.resources[key].value < resourcesToCast[key])
                {
                    return false;
                }
            }

            foreach (D_Resource key in resourcesToCast.Keys)
            {
                parent.resources[key].Consume(resourcesToCast[key]);
            }

            return true;
        }

        public DeepBehavior Clone()
        {
            DeepBehavior b = (DeepBehavior)this.MemberwiseClone();
            return b;
        }
    }
}