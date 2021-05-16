using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Linq;
namespace DeepAction
{

    [System.Serializable]
    [HideReferenceObjectPicker]
    public class DeepBehavior
    {
        //a behavior is just something that holds a bunch of actions.

        //all behaviors on an actionEntity are in the same list. they all get trigger on the same events. Except:
        //You can trigger a particular behavior with Trigger()

        public string behaviorID = "NOTSET";
        [HideInInspector]
        public DeepEntity parent;

        //resources require to TRIGGER the behavior. This is where you put the spell cost. Can be set by actions.
        public Dictionary<D_Resources, float> resourcesToTrigger = new Dictionary<D_Resources, float>();

        [TypeFilter("GetFilteredTypeList")]
        public List<DeepAction> actions = new List<DeepAction>();

        //this is NOT the same as Trigger target.
        //this can be SET by a DeepAction, and then used in a trigger, or whatever
        [HideInInspector]
        public DeepEntity behaviorTarget;
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

            foreach (DeepAction a in actions)
            {
                a.Trigger(point, direction, target);
            }
            return true;
        }
        #region Trigger Overloads
        public bool Trigger() { return Trigger(Vector3.zero, Vector3.zero, null); }
        public bool Trigger(Vector3 position) { return Trigger(position, Vector3.zero, null); }
        public bool Trigger(Vector3 position, Vector3 direction) { return Trigger(position, direction, null); }
        public bool Trigger(DeepEntity target) { return Trigger(Vector3.zero, Vector3.zero, target); }
        #endregion

        //communicate to another behavior. Clean this up
        public bool SendMessage(string message, DeepBehavior sender)
        {
            return false;
        }
        public virtual DeepBehavior Clone()
        {
            DeepBehavior newB = (DeepBehavior)this.MemberwiseClone();
            newB.actions = new List<DeepAction>();
            foreach (DeepAction a in this.actions)
            {
                DeepAction newA = a.Clone();
                newA.behavior = newB;
                newB.actions.Add(newA);
            }
            return newB;
        }


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