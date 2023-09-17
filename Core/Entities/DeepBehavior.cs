using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using Unity.Jobs;
namespace DeepAction
{
#if ODIN_INSPECTOR
    [DoNotDrawAsReference]
#endif
    public abstract class DeepBehavior
    {
#if ODIN_INSPECTOR
        [HideInEditorMode]
#endif
        /// <summary>The entity this behavior is on.</summary>
        public DeepEntity parent;
#if ODIN_INSPECTOR
        [HideInEditorMode]
#endif
        /// <summary>The entity that created this behavior. Behaviors added during Init are owned by the owner of the Entity.</summary>
        public DeepEntity owner;

        public virtual void InitializeBehavior() { }
        public virtual void DestroyBehavior() { }

        public DeepBehavior Clone()
        {
            DeepBehavior b = (DeepBehavior)this.MemberwiseClone();
            return b;
        }
    }

    // Abilities are just behaviors with a built in resource consumtion and cooldown.
    // Not 100% happy with this being a child class. Might combine back into DeepBehavior idk...

    public abstract class DeepAbility : DeepBehavior
    {
        public virtual Dictionary<D_Resource, int> resourcesToTrigger { get; private set; } = new Dictionary<D_Resource, int>();
        public virtual float triggerCooldown { get; protected set; } = 1f;

        public float lastTriggerTime { get; private set; } = -999999f;
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public float remainingCooldown => Mathf.Max(triggerCooldown - (Time.time - lastTriggerTime), 0f);
        public bool cooldownFinished => remainingCooldown <= 0f;//todo cache

        public bool Trigger()
        {
            if (!cooldownFinished)
            {
                return false;
            }

            foreach (D_Resource key in resourcesToTrigger.Keys)
            {
                if (parent.resources[key].value < resourcesToTrigger[key])
                {
                    return false;
                }
            }

            foreach (D_Resource key in resourcesToTrigger.Keys)
            {
                parent.resources[key].Consume(resourcesToTrigger[key]);
            }

            lastTriggerTime = Time.time;
            OnTrigger();
            return true;
        }

        public virtual void OnTrigger() { }
    }
}