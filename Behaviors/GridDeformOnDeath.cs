using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class GridDeformOnDeath : DeepBehavior
    {
        public float force;
        public float forceX;

        public GridDeformOnDeath(float force, float forceX)
        {
            this.force = force;
            this.forceX = forceX;
        }

        public override void InitializeBehavior()
        {
            parent.events.OnEntityDie += Deform;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnEntityDie -= Deform;
        }

        private void Deform()
        {
            GridShaderControl.instance.AddForceAtLocation(parent.transform.position, force, forceX);
        }
    }
}