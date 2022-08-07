using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    /// <summary>
    /// Contains all the events that a DeepEntity triggers. 
    /// </summary>
    public class DeepEntityEvents
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
}
