using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeepAction;
using Sirenix.OdinInspector;
using System;
public class AddBehaviorToEntity : MonoBehaviour
{
    public DeepEntity entity;
    public Type behavior;

    [Button]
    public void AddBehavior()
    {
        entity.AddBehavior<ExampleBehavior>();   
    }
}
