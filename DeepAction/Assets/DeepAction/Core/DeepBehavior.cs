using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Linq;
namespace DeepAction
{
    [System.Serializable][HideReferenceObjectPicker]
    public class DeepBehavior
    {
        //a behavior is just something that holds a bunch of actions.

        //all behaviors on an actionEntity are in the same list. they all invoke the same events. Except:
        //You can trigger a particular behavior with Trigger()

        public string behaviorID = "NOTSET";
        [Tooltip("Literally just a place to leave notes about the behavior. This should NOT be used for any functionality."),SerializeField,TextArea]
        private string notes = "";
        [Space]
        public Dictionary<D_Resources, float> resourcesToTrigger = new Dictionary<D_Resources, float>();//resources require to TRIGGER the behavior. This is where you put the spell cost. Can be set by actions.
        [TypeFilter("GetFilteredTypeList")][Space]
        public List<DeepAction> actions = new List<DeepAction>();


        [HideInInspector]
        public DeepEntity parent;
        [HideInInspector]
        public DeepEntity behaviorTarget;   //a target that a behavior HOLDS. this is NOT the same as the target that gets passed in Trigger()
        [HideInInspector]
        public Events events;

        public class Events
        {
            public Action<Vector3,Vector3,DeepEntity> Trigger;

            public Action OnEntityEnable;
            public Action OnEntityDisable;
            public Action OnEntityDie;

            public Action Update;
            public Action FixedUpdate;
            public Action LateUpdate;

            public Action<float>OnTakeDamage;
            public Action<float>OnDealDamage;
        }

        public bool Trigger(Vector3 point, Vector3 direction, DeepEntity target)
        {
            foreach (D_Resources key in resourcesToTrigger.Keys)
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

            foreach (D_Resources key in resourcesToTrigger.Keys)
            {
                parent.resources[key].TryToConsume(resourcesToTrigger[key]);
            }

            events.Trigger?.Invoke(point,direction,target);

            return true;
        }
        #region Trigger Overloads
        public bool Trigger() { return Trigger(Vector3.zero, Vector3.zero, null); }
        public bool Trigger(Vector3 position) { return Trigger(position, Vector3.zero, null); }
        public bool Trigger(Vector3 position, Vector3 direction) { return Trigger(position, direction, null); }
        public bool Trigger(DeepEntity target) { return Trigger(Vector3.zero, Vector3.zero, target); }
        #endregion

        public virtual DeepBehavior Clone()
        {
            DeepBehavior newB = (DeepBehavior)this.MemberwiseClone();
            newB.actions = new List<DeepAction>();

            newB.events = new Events();

            foreach (DeepAction a in this.actions)
            {
                DeepAction newA = a.Clone();
                newA.behavior = newB;
                newB.actions.Add(newA);
                newA.IntitializeAction();
            }
            return newB;
        }

        //for odin
        private IEnumerable<Type> GetFilteredTypeList()
        {
            var q = typeof(DeepAction).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
                .Where(x => !x.IsGenericTypeDefinition)                             // Excludes C1<>??
                .Where(x => typeof(DeepAction).IsAssignableFrom(x));                // Excludes classes not inheriting from BaseClass
            return q;
        }
    }
}