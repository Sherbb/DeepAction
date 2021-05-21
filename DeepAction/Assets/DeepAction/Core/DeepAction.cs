using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace DeepAction
{
    [System.Serializable]
    [HideReferenceObjectPicker]
    public abstract class DeepAction
    {
        [HideInInspector][Newtonsoft.Json.JsonIgnore]//mommy
        public DeepBehavior behavior;

        /// <summary>
        /// Called when a behavior is added to an entity
        /// </summary>
        public virtual void IntitializeAction() { }
        /// <summary>
        /// Called when a behavior is removed from an entity
        /// </summary>
        public virtual void DestroyAction() { }

        /// <summary>
        /// By default this is a shallow copy. Override if you need to do a deep copy.
        /// </summary>
        public virtual DeepAction Clone()
        {
            return (DeepAction)this.MemberwiseClone();
        }
    }

    public class TestAction : DeepAction
    {
        public override void IntitializeAction()
        {
            behavior.events.Trigger += Test;
            behavior.events.Update += Example;
        }

        public void Example()
        {
            Debug.Log(behavior.events.Update);
        }

        public void Test(Vector3 point, Vector3 direction, DeepEntity target)
        {
             Debug.Log("Ability triggered");
        }

        public override void DestroyAction()
        {
            behavior.events.Trigger -= Test;
            behavior.events.Update -= Example;
        }

    }
}