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
        [HideInInspector]//mommy
        public DeepBehavior behavior;

        public virtual void IntitializeAction() { }
        public virtual void DestroyAction() { }

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
        }

        public void Test(Vector3 point, Vector3 direction, DeepEntity target)
        {
             Debug.Log("Ability triggered");
        }

    }
}