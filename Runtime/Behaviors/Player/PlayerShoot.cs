using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DeepAction
{
    public class PlayerShoot : DeepAbility
    {
        public Func<EntityTemplate> template;
        private float _inaccuracy;

        /*
        public override Dictionary<D_Resource, int> resourcesToTrigger => new Dictionary<D_Resource, int>()
        {
            {D_Resource.Mana,1}
        };
        */

        public PlayerShoot(float cooldown, float inaccuracy, Func<EntityTemplate> template)
        {
            triggerCooldown = cooldown;
            this.template = template;
            _inaccuracy = inaccuracy;
        }

        public override void OnTrigger()
        {
            for (int i = 0; i < 2; i++)
            {
                DeepEntity e = DeepEntity.Create(
                    template.Invoke(),
                    parent,
                    parent.transform.position,
                    Quaternion.Euler(0f, 0f,
                        Mathf.Atan2(parent.aimDirection.y, parent.aimDirection.x) *
                        Mathf.Rad2Deg + Mathf.Sin((Time.time * 10f) + (float)i * Mathf.PI) * _inaccuracy * .5f)
                );
            }
        }
    }
}
