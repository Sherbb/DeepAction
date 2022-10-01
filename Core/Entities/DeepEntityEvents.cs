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

        // * Hearbeat
        public Action Update;
        public Action FixedUpdate;

        // * Collision & Movement
        //pass through of the standard unity events
        public Action<Vector2> OnBounce;//vec2 = bounce point
        public Action<Collision2D> OnCollisionEnter2D;
        public Action<Collision2D> OnCollisionExit2D;
        public Action<Collision2D> OnCollisionStay2D;

        //only passes when colliding with anotherDeepEntity
        public Action<DeepEntity> OnEntityCollisionEnter;
        public Action<DeepEntity> OnEntityCollisionExit;
        public Action<DeepEntity> OnEntityCollisionStay;

        // * Damage
        public Action<float> OnTakeDamage;
        public ActionRef<float> OnTakeDamageRef;
        public Action<float> OnDealDamage;
    }
}
