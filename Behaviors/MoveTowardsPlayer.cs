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
            if (App.state.game.entityByTeamAndTypeLookup[D_Team.Player][D_EntityType.Actor].list.Count > 0)
            {
                Vector2 move = App.state.game.entityByTeamAndTypeLookup[D_Team.Player][D_EntityType.Actor][0].transform.position - parent.transform.position;
                Vector2 force = move.normalized * parent.attributes[D_Attribute.MoveSpeed].value;
                parent.mb.SetVelocity(Vector2.Lerp(Vector2.ClampMagnitude(parent.mb.velocity, parent.mb.effectiveVelocity.magnitude), force, 5f * Time.deltaTime));
            }
        }
    }
}
