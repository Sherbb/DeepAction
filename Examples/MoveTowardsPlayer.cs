using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class MoveTowardsPlayer : DeepBehavior
    {
        public override void IntitializeBehavior()
        {
            parent.events.FixedUpdate += Move;
        }
        public override void DestroyBehavior()
        {
            parent.events.FixedUpdate -= Move;
        }
        void Move()
        {
            if (DeepManager.instance.entityByTeamAndTypeLookup[D_Team.Player][D_EntityType.Actor].Count > 0)
            {
                Vector2 force = DeepManager.instance.entityByTeamAndTypeLookup[D_Team.Player][D_EntityType.Actor][0].transform.position - parent.transform.position;
                force = force.normalized * Time.fixedDeltaTime * parent.attributes[D_Attribute.MoveSpeed].value;
                parent.rb.AddForce(force, ForceMode2D.Force);
            }
        }
    }
}
