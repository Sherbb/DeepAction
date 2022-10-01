using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class MoveForwards : DeepBehavior
    {
        public override void IntitializeBehavior()
        {
            parent.events.OnEntityEnable += Move;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnEntityEnable -= Move;
        }

        private void Move()
        {
            parent.mb.AddForce(parent.transform.right * parent.attributes[D_Attribute.MoveSpeed].value);
        }
    }
}
