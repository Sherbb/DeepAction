using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class AreaBehaviorOnDeath : DeepBehavior
    {
        private float radius;
        private DeepBehavior behavior;
        private D_Team targetTeam;

        public AreaBehaviorOnDeath(float radius, DeepBehavior behavior, D_Team targetTeam)
        {
            this.radius = radius;
            this.behavior = behavior;
            this.targetTeam = targetTeam;
        }

        public override void InitializeBehavior()
        {
            parent.events.OnEntityDie += OnDie;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnEntityDie -= OnDie;
        }

        private void OnDie()
        {
            DeepActions.AreaBehavior(parent.transform.position, radius, behavior, targetTeam);
        }
    }
}