using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class MoveForwards : DeepBehavior
    {
        public override void IntitializeBehavior()
        {
            parent.events.FixedUpdate += Move;
        }

        public override void DestroyBehavior()
        {
            parent.events.FixedUpdate -= Move;
        }

        private void Move()
        {
            parent.rb.AddForce(parent.transform.right * parent.attributes[D_Attribute.MoveSpeed].value, ForceMode2D.Force);
        }
    }
}
