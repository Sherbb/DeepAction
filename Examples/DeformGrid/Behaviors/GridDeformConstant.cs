using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    /// <summary>
    /// DISABLED
    /// </summary>
    public class GridDeformConstant : DeepBehavior
    {
        public float force;
        public float forceX;

        public GridDeformConstant(float force, float forceX)
        {
            this.force = force;
            this.forceX = forceX;
        }

        public override void InitializeBehavior()
        {
            parent.events.FixedUpdate += Deform;
        }

        public override void DestroyBehavior()
        {
            parent.events.FixedUpdate -= Deform;
        }

        private void Deform()
        {
            GridShaderControl.instance.AddForceAtLocation(parent.transform.position, force, forceX);
        }
    }
}