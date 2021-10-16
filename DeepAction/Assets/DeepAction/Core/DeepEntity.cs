using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

namespace DeepAction
{
    public class DeepEntity : MonoBehaviour, IHit
    {
        // * Preset
        public DeepEntityPreset preset;

        // * Resources
        [Title("Resources", "", TitleAlignments.Centered)]
        [Space,HideInEditorMode]
        public Dictionary<D_Resource, DeepResource> resources = new Dictionary<D_Resource, DeepResource>();

        // * Attributes
        [Title("Attributes", "", TitleAlignments.Centered)]
        [Space,HideInEditorMode]
        public Dictionary<D_Attribute, DeepAttribute> attributes = new Dictionary<D_Attribute, DeepAttribute>();

        // * Behaviors
        [Title("Behaviors", "", TitleAlignments.Centered)]
        [Space,HideInEditorMode]
        public List<DeepBehavior> behaviors = new List<DeepBehavior>();

////////////////////////////////////////////////////////////////////////////////////////////////

        [HideInInspector]
        public Action OnDeath;

        public static readonly D_Resource[] damageHeirarchy = {D_Resource.Health};//Damage is done from left to right    This does not have to be static.        

        private bool hasDied;//used to track for after death

        public Events events;
        public class Events
        {//seperate class just to organize.

            public Action<Vector3,Vector3,DeepEntity> Trigger;

            public Action OnEntityEnable;
            public Action OnEntityDisable;
            public Action OnEntityDie;

            public Action Update;
            public Action FixedUpdate;
            public Action LateUpdate;

            public Action<float> OnTakeDamage;
            public ActionRef<float> OnTakeDamageRef;
            public Action<float> OnDealDamage;
        }

        private void OnEnable()//having this on enable has huge implications that you may not be ok with....
        {
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
            foreach (DeepResource res in resources.Values)
            {
                res.parentEntity = this;
                res.SetValueWithRatio(res.defaultRatio);
            }
            hasDied = false;
        }

        public DeepBehavior AddBehavior(Type T)
        {
            if (T.IsAssignableFrom(typeof(DeepBehavior)))
            {
                DeepBehavior b = (DeepBehavior)Activator.CreateInstance(T);
                behaviors.Add(b);
                return b;
            }
            else
            {
                Debug.LogError("Non DeepBehavior type used in AddBehavior:  " + T.ToString());
                return null;
            }
        }

        public DeepBehavior AddBehavior(DeepBehavior behavior)
        {
            DeepBehavior b = behavior;
            b.parent = this;
            behaviors.Add(b);

            b.IntitializeBehavior();

            return b;
        }

        /// <summary>
        /// Removes the behavior from the entity and calls DestroyAction() on all actions.
        /// </summary>
        /// <returns>Returns true if the enity had the behavior</returns>
        public bool RemoveBehavior(DeepBehavior behavior)
        {
            if (!behaviors.Contains(behavior))
            {
                return false;
            }
            behaviors.Remove(behavior);
            return true;
        }

        //End Maintanance stuff.

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
            OnDeath?.Invoke();

            foreach (DeepBehavior b in behaviors)
            {
                events.OnEntityDie?.Invoke();
            }

            hasDied = true;

            gameObject.SetActive(false);
        }

        //standard unity stuff
        private void Update()
        {
            foreach (DeepResource res in resources.Values)
            {
                res.Tick();
            }
            //If you make the damageHeirarchy non const or empty, you need to update this. 
            if (resources[damageHeirarchy[damageHeirarchy.Length - 1]].GetValue() <= 0)
            {
                //resources can have -regen. 
                //So if your [hp] has negative regen we wanna see if you died here
                Die();
            }

            foreach (DeepBehavior b in behaviors)
            {
                events.Update?.Invoke();
            }
        }

        private void FixedUpdate()
        {
            foreach (DeepBehavior b in behaviors)
            {
                events.FixedUpdate?.Invoke();
            }
        }

        private void LateUpdate()
        {
            foreach (DeepBehavior b in behaviors)
            {
                events.LateUpdate?.Invoke();
            }
        }

        private void OnDisable()
        {
            foreach (DeepBehavior b in behaviors)
            {
                events.OnEntityDisable?.Invoke();
            }
        }
    }
}