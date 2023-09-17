using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class SuckNearbyArtifacts : DeepBehavior
    {
        private DeepEntity[] artifactBuffer = new DeepEntity[40];//can only suck this many artifacts at a time.
        private float radius;
        private float force;

        private D_EntityType[] typeFilter = new D_EntityType[] { D_EntityType.Actor };
        private D_Team[] teamFilter = new D_Team[] { D_Team.Artifact };

        public SuckNearbyArtifacts(float radius, float force)
        {
            this.radius = radius;
            this.force = force;
        }

        // TODO rework this such that we read on collision input etc...

        public override void InitializeBehavior()
        {
            parent.events.UpdateNorm += OnUpdate;
        }

        public override void DestroyBehavior()
        {
            parent.events.UpdateNorm -= OnUpdate;
        }

        private void OnUpdate()
        {
            int hits = DeepUtility.GetEntitiesInAreaNonAlloc(parent.transform.position, radius, artifactBuffer, typeFilter, teamFilter);
            for (int i = 0; i < hits; i++)
            {
                var mb = artifactBuffer[i].mb;
                mb.AddForce((parent.transform.position - artifactBuffer[i].transform.position).normalized * force * Time.deltaTime);
                mb.SetVelocity(mb.velocity.magnitude * (parent.transform.position - mb.transform.position).normalized);
            }
        }
    }
}
