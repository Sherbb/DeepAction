using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class SuckArtifactsAbility : DeepAbility
    {
        private float aStrength;
        private float eStrength;

        public SuckArtifactsAbility(float artifactPullStrength, float enemlyPushStrength, float cooldown)
        {
            triggerCooldown = cooldown;
            this.aStrength = artifactPullStrength;
            this.eStrength = artifactPullStrength;
        }

        public override void OnTrigger()
        {
            if (parent.resources[D_Resource.Artifacts].isFull)
            {
                return;
            }

            foreach (DeepEntity e in App.state.game.entityByTeamAndTypeLookup[D_Team.Artifact][D_EntityType.Actor])
            {
                e.mb.AddForce((parent.transform.position - e.transform.position).normalized * aStrength);
            }

            foreach (DeepEntity e in App.state.game.entityByTeamAndTypeLookup[D_Team.Enemy][D_EntityType.Actor])
            {
                e.mb.AddForce((parent.transform.position - e.transform.position).normalized * -eStrength);
                e.AddBehavior(new DecayingAttributeMod(D_Attribute.MoveSpeed, new ModValues(0f, -.9f, 0f), 3f), owner);
            }
        }
    }
}