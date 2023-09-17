using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class ArtifactPickup : DeepBehavior
    {
        //if an entity collides and has artifact space, artifact is destroyed and entity gets +1 artifact

        public override void InitializeBehavior()
        {
            parent.events.OnEntityCollisionEnter += OnCollision;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnEntityCollisionEnter -= OnCollision;
        }

        private void OnCollision(DeepEntity other)
        {
            if (other.type == D_EntityType.Projectile || other.team != D_Team.Player)
            {
                return;
            }
            parent.Die();
            DeepResource r = other.resources[D_Resource.Artifacts];
            /*
            if (r.currentMax > 0 && r.value < r.currentMax)
            {
                r.Regen(1);
                parent.Die();
            }
            */
        }
    }
}