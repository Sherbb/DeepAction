using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class BasicProjectile : DeepBehavior
    {
        private int _impactDamage;
        private D_Team[] _targetTeam;

        public BasicProjectile(int impactDamage, params D_Team[] targetTeam)
        {
            _impactDamage = impactDamage;
            _targetTeam = targetTeam;
        }

        public override void IntitializeBehavior()
        {
            parent.events.OnEntityCollisionEnter += HandleCollision;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnEntityCollisionEnter -= HandleCollision;
        }

        private void HandleCollision(DeepEntity e)
        {
            foreach (D_Team t in _targetTeam)
            {
                if (t == e.team)
                {
                    e.Hit(new Damage(_impactDamage));
                    parent.Die();
                    return;
                }
            }
        }
    }
}
