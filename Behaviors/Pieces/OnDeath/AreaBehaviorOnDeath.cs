using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class AreaBehaviorOnDeath : DeepBehavior
    {
        public float radius;
        public DeepBehavior behavior;
        public D_Team targetTeam;
        public bool applyDublicates;

        public AreaBehaviorOnDeath(float radius, DeepBehavior behavior, D_Team targetTeam, bool applyDublicates)
        {
            this.radius = radius;
            this.behavior = behavior;
            this.targetTeam = targetTeam;
            this.applyDublicates = applyDublicates;
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
            DeepActions.AreaBehavior(parent.transform.position, radius, behavior, targetTeam, applyDublicates);
        }
    }
}