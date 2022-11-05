using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DeepAction
{
    public class PlayerShoot : DeepAbility
    {
        public Func<EntityTemplate> template;

        /*
        public override Dictionary<D_Resource, int> resourcesToTrigger => new Dictionary<D_Resource, int>()
        {
            {D_Resource.Mana,1}
        };
        */

        public PlayerShoot(float cooldown, Func<EntityTemplate> template)
        {
            triggerCooldown = cooldown;
            this.template = template;
        }

        public override void OnTrigger()
        {
            DeepEntity e = GameObject.Instantiate(Resources.Load("Projectile") as GameObject,
                parent.transform.position,
                Quaternion.Euler(
                    0f, 0f, Mathf.Atan2(parent.aimDirection.y, parent.aimDirection.x) * Mathf.Rad2Deg)
                ).GetComponent<DeepEntity>();

            e.Initialize(template.Invoke());
        }
    }
}
