using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class RandomStartVelocity : DeepBehavior
    {
        public override void InitializeBehavior()
        {
            parent.events.OnEntityEnable += Move;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnEntityEnable -= Move;
        }

        private void Move()
        {
            Vector2 dir = Random.insideUnitCircle.normalized;

            parent.mb.AddForce(dir * parent.attributes[D_Attribute.MoveSpeed].value);
        }
    }
}