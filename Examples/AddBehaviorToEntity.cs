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
    public void AddBehavior()
    {
        entity.AddBehavior(behavior);
    }
}
