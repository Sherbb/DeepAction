using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{

    public class AvoidOtherEntities : DeepBehavior
    {
        public D_Team teamToAvoid;
        public D_EntityType typeToAvoid;
        public float avoidStrength;

        public AvoidOtherEntities(D_Team team, D_EntityType type, float force)
        {
            teamToAvoid = team;
            typeToAvoid = type;
            avoidStrength = force;
        }

        public override void IntitializeBehavior()
        {
            parent.events.OnEntityCollisionStay += Avoid;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnEntityCollisionStay -= Avoid;
        }

        private void Avoid(DeepEntity e)
        {
            if (e.team == teamToAvoid && e.type == typeToAvoid)
            {
                e.mb.AddForce((e.transform.position - parent.transform.position).normalized * avoidStrength * Time.deltaTime);
            }
        }
    }
}
