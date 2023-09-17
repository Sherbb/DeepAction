using System;
using UnityEngine;

namespace DeepAction
{
    /// <summary>
    /// Creates an entity aimed at the closest entity to the player.
    /// </summary>
    public class ShootClosestEnemy : DeepBehavior
    {
        public float cooldown;
        public float inaccuracy;
        public Func<EntityTemplate> template;
        private float _timer;

        public ShootClosestEnemy(float cooldown, float inaccuracy, Func<EntityTemplate> template)
        {
            this.cooldown = cooldown;
            this.inaccuracy = inaccuracy;
            this.template = template;
        }

        public override void InitializeBehavior()
        {
            parent.events.UpdateNorm += Update;
        }

        public override void DestroyBehavior()
        {
            parent.events.UpdateNorm -= Update;
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (App.state.game.closestEnemyActor.value == null)
            {
                _timer = Mathf.Clamp(_timer, 0f, cooldown);
                return;
            }

            if (_timer >= cooldown)
            {
                _timer -= cooldown;
                Fire();
            }
        }

        public void Fire()
        {
            //not normailzed
            Vector3 directionToClosestEnemy = App.state.game.closestEnemyActor.value.cachedTransform.position - parent.cachedTransform.position;

            DeepEntity e = DeepEntity.Create(
                template.Invoke(),
                parent,
                parent.cachedTransform.position,
                Quaternion.Euler(0f, 0f,
                    Mathf.Atan2(directionToClosestEnemy.y, directionToClosestEnemy.x) * Mathf.Rad2Deg +
                    UnityEngine.Random.Range(-1f, 1f) * inaccuracy * .5f)
            );
        }
    }
}