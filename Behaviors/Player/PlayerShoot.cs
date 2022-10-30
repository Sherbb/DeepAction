using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class PlayerShoot : DeepAbility
    {
        private int _damage;

        /*
        public override Dictionary<D_Resource, int> resourcesToTrigger => new Dictionary<D_Resource, int>()
        {
            {D_Resource.Mana,1}
        };
        */

        public PlayerShoot(int damage, float cooldown)
        {
            _damage = damage;
            triggerCooldown = cooldown;
        }

        public override void OnTrigger()
        {
            DeepEntity e = GameObject.Instantiate(Resources.Load("Projectile") as GameObject,
                parent.transform.position,
                Quaternion.Euler(
                    0f, 0f, Mathf.Atan2(parent.aimDirection.y, parent.aimDirection.x) * Mathf.Rad2Deg)
                ).GetComponent<DeepEntity>();

            e.Initialize(DeepEntityPresets.ExamplePlayerProjectile(_damage));
        }
    }
}
