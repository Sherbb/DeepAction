using System;
using UnityEngine;

namespace DeepAction
{
    /// <summary>
    /// Contains all the events that a DeepEntity triggers. 
    /// </summary>
    public class DeepEntityEvents
    {
        public Action OnEntityEnable;
        public Action OnEntityDisable;
        public Action OnEntityDie;

        // * Hearbeat
        public Action Update;
        public Action FixedUpdate;

        // * Collision & Movement
        public Action<Vector2> OnBounce;//vec2 = bounce point

        //pass through of the standard unity events
        public Action<Collider2D> OnTriggerEnter2D;
        public Action<Collider2D> OnTriggerExit2D;
        public Action<Collider2D> OnTriggerStay2D;

        //only passes when colliding with anotherDeepEntity
        public Action<DeepEntity> OnEntityCollisionEnter;
        public Action<DeepEntity> OnEntityCollisionExit;
        public Action<DeepEntity> OnEntityCollisionStay;

        // * Damage
        public Action<float> OnTakeDamage;
        public Action<float> OnDealDamage;//todo
    }
}
