using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

namespace DeepAction
{
    public class DeepEntity : MonoBehaviour, IHit
    {
        // * Resources
        [Title("Resources", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
        public Dictionary<D_Resource, DeepResource> resources { get; private set; }
        // * Attributes
        [Title("Attributes", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
        public Dictionary<D_Attribute, DeepAttribute> attributes { get; private set; }
        // * States
        [Title("States", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
        public Dictionary<D_State, DeepState> states { get; private set; }
        // * Behaviors
        [Title("Behaviors", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
        public List<DeepBehavior> behaviors { get; private set; }
        // * Events
        public DeepEntityEvents events;
        // * Flags
        [HideInInspector]
        //todo
        public bool dying { get; private set; }//entity will be killed(disabled) next LateUpdate()

        ////////////////////////////////////////////////////////////////////////////////////////////////

        public static readonly D_Resource[] damageHeirarchy = { D_Resource.Health };//Damage is done from left to right       

        public Rigidbody2D rb { get; private set; }
        private EntityTemplate template;

        public DeepEntity Initialize(EntityTemplate t)
        {
            template = t;

            events = new DeepEntityEvents();
            attributes = new Dictionary<D_Attribute, DeepAttribute>();
            resources = new Dictionary<D_Resource, DeepResource>();
            states = new Dictionary<D_State, DeepState>();
            rb = gameObject.AddComponent<Rigidbody2D>();

            foreach (A att in template.attributes)
            {
                this.AddAttribute(att);
            }
            //fill in missing attributes
            foreach (D_Attribute a in Enum.GetValues(typeof(D_Attribute)))
            {
                if (!attributes.ContainsKey(a))
                {
                    this.AddAttribute(a, new DeepAttribute(0f));
                }
            }
            foreach (R res in template.resources)
            {
                this.AddResource(res);
            }
            //fill in missing resources
            foreach (D_Resource r in Enum.GetValues(typeof(D_Resource)))
            {
                if (!resources.ContainsKey(r))
                {
                    this.AddResource(r, new DeepResource(1, 0));
                }
            }
            foreach (D_State s in Enum.GetValues(typeof(D_State)))
            {
                this.AddState(s);
            }
            foreach (DeepBehavior b in template.behaviors)
            {
                this.AddBehavior(b);
            }

            return this;
        }


        private void OnEnable()
        {
            dying = false;

            foreach (R r in template.resources)
            {
                resources[r.type].SetValue(r.baseValue);
            }

            DeepManager.instance.activeEntities.Add(this);
        }

        void OnDisable()
        {
            dying = false;
            events.OnEntityDisable?.Invoke();
            DeepManager.instance.activeEntities.Remove(this);
        }

        public DeepBehavior AddBehavior<T>() where T : DeepBehavior
        {
            DeepBehavior b = (DeepBehavior)Activator.CreateInstance(typeof(T));
            return AddBehavior(b);
        }

        public DeepBehavior AddBehavior(Type behavior)
        {
            if (!typeof(DeepBehavior).IsAssignableFrom(behavior)) return null;// >:(
            DeepBehavior b = (DeepBehavior)Activator.CreateInstance(behavior);
            return AddBehavior(b);
        }

        public DeepBehavior AddBehavior(DeepBehavior behavior)
        {
            behavior.parent = this;
            behaviors.Add(behavior);
            behavior.IntitializeBehavior();
            return behavior;
        }

        public bool RemoveBehavior<T>() where T : DeepBehavior
        {
            foreach (T b in behaviors.OfType<T>())
            {
                b.DestroyBehavior();
                behaviors.Remove(b);
                return true;
            }
            return false;
        }

        public bool RemoveBehavior(DeepBehavior behavior)
        {
            if (!behaviors.Contains(behavior))
            {
                return false;
            }
            behavior.DestroyBehavior();
            behaviors.Remove(behavior);
            return true;
        }

        public float GetAttributeValue(D_Attribute attribute)
        {
            if (attributes.ContainsKey(attribute))
            {
                return attributes[attribute].value;
            }
            return 0f;
        }

        public bool TryGetAttributeValue(D_Attribute attribute, out float value)
        {
            if (attributes.ContainsKey(attribute))
            {
                value = attributes[attribute].value;
                return true;
            }
            value = 0f;
            return false;
        }

        public void Hit(float damage)
        {
            float d = damage;
            foreach (DeepBehavior b in behaviors)
            {
                events.OnTakeDamage?.Invoke(d);
                events.OnTakeDamageRef?.Invoke(ref d);
            }
            //todo
        }

        public void Die()
        {
            events.OnEntityDie?.Invoke();
            dying = true;
        }
    }
}