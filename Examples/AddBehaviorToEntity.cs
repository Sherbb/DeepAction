using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeepAction;
using Sirenix.OdinInspector;
using System;
public class AddBehaviorToEntity : SerializedMonoBehaviour
{
    public DeepEntity entity;
    public Type behavior;

    [Button]
    public void AddBehaviorTypeVariable()
    {
        entity.AddBehavior(behavior);
    }

    public void AddBehaviorType()
    {
        entity.AddBehavior<DecayingSlow>();
    }

    public void AddBehaviorRef()
    {
        DeepBehavior b = new DecayingSlow();
        entity.AddBehavior(b);
    }
}
