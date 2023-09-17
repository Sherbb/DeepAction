using UnityEngine;

namespace DeepAction
{
    public class BasicProjectile : DeepBehavior
    {
        public int _impactDamage;
        public D_Team[] _targetTeam;

        public BasicProjectile(int impactDamage, params D_Team[] targetTeam)
        {
            _impactDamage = impactDamage;
            _targetTeam = targetTeam;
        }

        public override void InitializeBehavior()
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
                    if (_impactDamage > 0)
                    {
                        e.Hit(new Damage(_impactDamage, Color.cyan));
                    }
                    parent.Die();
                    return;
                }
            }
        }
    }
}
