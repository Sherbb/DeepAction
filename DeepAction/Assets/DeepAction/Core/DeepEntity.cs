using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Linq;

namespace DeepAction
{
    public class DeepEntity : MonoBehaviour, IHit
    {
        // * Preset
        public DeepEntityPreset preset;

        // * Resources
        [Title("Resources", "", TitleAlignments.Centered)]
        [Space, HideInEditorMode, ShowInInspector]
        public Dictionary<D_Resource, DeepResource> resources = new Dictionary<D_Resource, DeepResource>();

        // * Attributes
        [Title("Attributes", "", TitleAlignments.Centered)]
        [Space, HideInEditorMode, ShowInInspector]
        public Dictionary<D_Attribute, DeepAttribute> attributes = new Dictionary<D_Attribute, DeepAttribute>();

        // * States
        [Title("States", "", TitleAlignments.Centered)]
        [Space,HideInEditorMode,ShowInInspector]
        public Dictionary<D_State, DeepState> states = new Dictionary<D_State, DeepState>();

        // * Behaviors
        [Title("Behaviors", "", TitleAlignments.Centered)]
        [Space, HideInEditorMode, ShowInInspector]
        public List<DeepBehavior> behaviors = new List<DeepBehavior>();

        ////////////////////////////////////////////////////////////////////////////////////////////////

        public static readonly D_Resource[] damageHeirarchy = { D_Resource.Health };//Damage is done from left to right       

        // * Flags
        public bool dying;//entity will be killed next LateUpdate()

        public Events events;
        public class Events
        {
            public Action<Vector3, Vector3, DeepEntity> Trigger;//idk...

            public Action OnEntityEnable;
            public Action OnEntityDisable;
            public Action OnEntityDie;

            public Action Update;
            public Action FixedUpdate;

            public Action<float> OnTakeDamage;
            public ActionRef<float> OnTakeDamageRef;
            public Action<float> OnDealDamage;
        }

        void Awake()
        {
            events = new Events();
        }

        private void OnEnable()//having this on enable has huge implications that you may not be ok with....
        {
            dying = false;
            if (preset != null)
            {
                attributes = new Dictionary<D_Attribute, DeepAttribute>(preset.attributes);
                foreach (D_Attribute key in preset.attributes.Keys)
                {
                    attributes[key] = attributes[key].Clone();
                }
                resources = new Dictionary<D_Resource, DeepResource>(preset.resources);
                foreach (D_Resource key in preset.resources.Keys)
                {
                    resources[key] = resources[key].Clone();
                }
                behaviors = new List<DeepBehavior>();
                foreach (DeepBehavior b in preset.behaviors)
                {
                    AddBehavior(b);
                }
            }
            else
            {
                attributes = new Dictionary<D_Attribute, DeepAttribute>();
                resources = new Dictionary<D_Resource, DeepResource>();
                behaviors = new List<DeepBehavior>();
            }
            states = new Dictionary<D_State, DeepState>();
            foreach(D_State state in Enum.GetValues(typeof(D_State)))
            {
                states.Add(state,new DeepState());
            }
            foreach (DeepResource res in resources.Values)
            {
                res.parentEntity = this;
                res.SetValueWithRatio(res.defaultRatio);
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
            b.parent = this;
            behaviors.Add(b);
            b.IntitializeBehavior();
            return b;
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
            behaviors.Remove(behavior);
            return true;
        }

        public float GetAttributeValue(D_Attribute attribute)
        {
            if (attributes.ContainsKey(attribute))
            {
                return attributes[attribute].GetValue();
            }
            return 0f;
        }

        public bool TryGetAttributeValue(D_Attribute attribute, out float value)
        {
            if (attributes.ContainsKey(attribute))
            {
                value = attributes[attribute].GetValue();
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

            if (damageHeirarchy.Length == 0)
            {
                Die();
            }

            for (int i = 0; i < damageHeirarchy.Length; i++)
            {
                d = resources[damageHeirarchy[i]].ConsumeWithRemainder(d);
                if (d <= 0f)
                {
                    break;
                }
            }

            if (resources[damageHeirarchy[damageHeirarchy.Length - 1]].GetValue() <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            foreach (DeepBehavior b in behaviors)
            {
                events.OnEntityDie?.Invoke();
            }
            dying = true;
        }
    }
}