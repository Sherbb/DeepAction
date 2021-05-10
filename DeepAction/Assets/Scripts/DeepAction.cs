using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[System.Serializable][HideReferenceObjectPicker]
public abstract class DeepAction 
{
    [HideInInspector]//mommy
    public DeepBehavior behavior;
    //maybe pass like a utility vec4 or something.
    public virtual void Trigger(Vector3 point,Vector3 direction,ActionEntity target){ }
    /// <summary>
    /// Trigger was called on another behavior
    /// </summary>
    public virtual void OtherBehaviorTriggered(DeepBehavior otherBehavior){ }


    public virtual void IntitializeAction(){ }
    public virtual void DestroyAction(){ }

    public virtual void TakeDamage(){ }
    public virtual void DealDamage(){ }
    public virtual void Update(){ }
    public virtual void FixedUpdate(){ }


    public virtual void OnActionEntityEnable(){ }
    public virtual void OnActionEntityDisable(){ }
    public virtual void OnActionEntityDie(){ }

    public virtual DeepAction Clone()
    {
        return (DeepAction)this.MemberwiseClone();
    }
}

public class TestAction : DeepAction
{
    public override void Trigger(Vector3 point, Vector3 direction, ActionEntity target)
    {
        Debug.Log("Ability triggered");
    }
}