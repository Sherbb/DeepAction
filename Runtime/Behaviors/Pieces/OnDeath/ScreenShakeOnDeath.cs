using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DeepAction
{
    public class ScreenShakeOnDeath : DeepBehavior
    {
        private float _force;
        public ScreenShakeOnDeath(float force = 1f)
        {
            _force = force;
        }

        public override void InitializeBehavior()
        {
            parent.events.OnEntityDie += OnDeath;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnEntityDie -= OnDeath;
        }

        private void OnDeath()
        {
            CameraShaker.instance.Shake(parent.transform.position, _force);
        }
    }
}
