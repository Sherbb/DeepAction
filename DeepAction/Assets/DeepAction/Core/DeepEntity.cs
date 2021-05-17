using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
namespace DeepAction
{
    public class DeepEntity : SerializedMonoBehaviour, IHit
    {
        ///An action entity has:
        ///Resources
        ///Attributes
        ///Behaviors

        ///Behaviors can be:
        ///Abilities
        ///Status Effects
        ///Modifiers
        ///Whatever you can imagine

        ///An action entity is:
        ///
        ///the player
        ///ability projectiles
        ///enemies
        ///structures


        [Title("Deep Entity","Deep Action by @AlanSherba",TitleAlignments.Centered)]

        public Dictionary<D_Resources, DeepResource> resources = new Dictionary<D_Resources, DeepResource>();

        [Title("Attributes","",TitleAlignments.Centered)]

        public Dictionary<D_Attribute, DeepAttribute> attributes = new Dictionary<D_Attribute, DeepAttribute>();

        [Title("Behaviors","",TitleAlignments.Centered)]

        [System.NonSerialized, OdinSerialize]
        public List<DeepBehavior> behaviors = new List<DeepBehavior>();



        public DeepBehavior AddBehavior(DeepBehavior behavior)
        {
            DeepBehavior b = behavior.Clone();
            b.parent = this;
            behaviors.Add(b);

            b.IntitializeBehavior();

            return b;
        }


        //End Maintanance stuff.

        //Start behavior event stuff

        public void Hit(float damage)
        {
            foreach(DeepBehavior b in behaviors)
            {
                b.events.OnTakeDamage?.Invoke(damage);
            }
        }

        //standard unity stuff
        private void Update()
        {
            foreach(DeepBehavior b in behaviors)
            {
                b.events.Update?.Invoke();
            }
        }
        private void FixedUpdate()
        {
            foreach(DeepBehavior b in behaviors)
            {
                b.events.FixedUpdate?.Invoke();
            }
        }
        private void LateUpdate()
        {
            foreach(DeepBehavior b in behaviors)
            {
                b.events.LateUpdate?.Invoke();
            }
        }
        private void OnEnable()
        {
            foreach(DeepBehavior b in behaviors)
            {
                b.events.OnEntityEnable?.Invoke();
            }
        }
        private void OnDisable()
        {
            foreach(DeepBehavior b in behaviors)
            {
                b.events.OnEntityDisable?.Invoke();
            }
        }
    }
}