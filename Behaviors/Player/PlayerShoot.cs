using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class PlayerShoot : DeepBehavior
    {
        public override void IntitializeBehavior()
        {
            parent.events.Update += QueryInput;
        }
        public override void DestroyBehavior()
        {
            parent.events.Update -= QueryInput;
        }

        private void QueryInput()
        {
            if (Input.GetMouseButtonDown(0))
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
}
