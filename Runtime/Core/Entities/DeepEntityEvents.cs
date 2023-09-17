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

        // * Heartbeat
        /// <summary> Early is for more system-level tasks, You should avoid using it in behaviors </summary>
        public Action UpdateEarly;
        public Action UpdateSchedule;
        public Action UpdateNorm;
        public Action UpdateComplete;
        public Action UpdateFinal;

        public Action FixedUpdate;

        // * Collision & Movement
        public Action<Vector2> OnBounce;//vec2 = bounce point

        //pass through of the standard unity events
        public Action<Collider2D> OnTriggerEnter2D;
        public Action<Collider2D> OnTriggerExit2D;
        //public Action OnTriggerStay2D;

        //only passes when colliding with anotherDeepEntity
        public Action<DeepEntity> OnEntityCollisionEnter;
        public Action<DeepEntity> OnEntityCollisionExit;
        /// <summary> Called Before Right before UpdateNorm, Called once if entity is colliding with any number of other entities </summary>
        public Action OnEntityCollisionStay;

        // * Damage
        //specifically damage to HEALTH
        public Action<float> OnTakeDamage;
        public Action<float> OnDealDamage;
    }
}
