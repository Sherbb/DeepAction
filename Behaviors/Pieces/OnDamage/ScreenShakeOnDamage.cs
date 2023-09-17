using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class ScreenShakeOnDamage : DeepBehavior
    {
        private float _force;
        private bool _scaleByDamage;

        public ScreenShakeOnDamage(bool scaleByDamage, float force = 10f)
        {
            _scaleByDamage = scaleByDamage;
            _force = force;
        }

        public ScreenShakeOnDamage(float force = 10f)
        {
            _scaleByDamage = false;
            _force = force;
        }

        public override void InitializeBehavior()
        {
            parent.events.OnTakeDamage += OnDamage;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnTakeDamage -= OnDamage;
        }

        private void OnDamage(float damage)
        {
            CameraShaker.instance.Shake(parent.transform.position, _scaleByDamage ? _force * damage : _force);
        }
    }
}
