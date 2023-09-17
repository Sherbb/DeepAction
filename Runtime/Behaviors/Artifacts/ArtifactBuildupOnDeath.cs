using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    /// <summary>
    /// every time artifact buildup = 1 an artifact is created.
    /// </summary>
    public class ArtifactBuildupOnDeath : DeepBehavior
    {
        public float buildup;

        public ArtifactBuildupOnDeath(float buildup)
        {
            this.buildup = buildup;
        }

        public override void InitializeBehavior()
        {
            parent.events.OnEntityDie += OnDeath;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnEntityDie -= OnDeath;
        }

        private void OnDeath()
        {
            App.state.game.artifacts.ArtifactBuildup(buildup, parent.transform.position);
        }
    }
}