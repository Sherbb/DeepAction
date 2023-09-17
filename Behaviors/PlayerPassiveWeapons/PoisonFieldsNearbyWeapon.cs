using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DeepAction
{
    public class PoisonFieldsNearbyWeapon : DeepBehavior
    {
        public Func<EntityTemplate> template;
        public float spawnRadius;
        public float zoneRadius;
        public float timeBetweenSpawns;

        private Coroutine coroutine;

        public PoisonFieldsNearbyWeapon(Func<EntityTemplate> template, float spawnRadius, float zoneRadius, float timeBetweenSpawns)
        {
            this.template = template;
            this.spawnRadius = spawnRadius;
            this.timeBetweenSpawns = timeBetweenSpawns;
            this.zoneRadius = zoneRadius;
        }

        public override void InitializeBehavior()
        {
            coroutine = parent.StartCoroutine(WeaponCo());
        }

        public override void DestroyBehavior()
        {
            parent.StopCoroutine(coroutine);
        }

        private IEnumerator WeaponCo()
        {
            while (true)
            {
                yield return new WaitForSeconds(timeBetweenSpawns);
                Vector3 pos = parent.cachedTransform.position + (Vector3)(Random.insideUnitCircle * spawnRadius);
                DeepEntity.Create(template.Invoke(), owner, pos, Quaternion.identity, Vector3.one * zoneRadius);
            }
        }
    }
}