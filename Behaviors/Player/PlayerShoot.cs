using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class PlayerShoot : DeepBehavior
    {
        public override bool canBeCast => true;
        public override Dictionary<D_Resource, int> resourcesToCast => new Dictionary<D_Resource, int>()
        {
            {D_Resource.Mana,1}
        };

        public override void OnCast()
        {
            DeepEntity e = GameObject.Instantiate(Resources.Load("Projectile") as GameObject,
                parent.transform.position,
                Quaternion.Euler(
                    0f, 0f, Mathf.Atan2(parent.aimDirection.y, parent.aimDirection.x) * Mathf.Rad2Deg)
                ).GetComponent<DeepEntity>();

            e.Initialize(DeepEntityPresets.ExamplePlayerProjectile());
        }
    }
}
