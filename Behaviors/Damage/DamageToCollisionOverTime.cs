using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeepAction.VFX;

namespace DeepAction
{
    public class DamageToCollisionOverTime : DeepBehavior
    {
        public Damage damage;
        public float timeBetweenTicks;
        public bool tickImmediatly;
        public DeepVFXAction[] vfxActions;
        public D_Team targetTeam;
        public D_EntityType targetType;

        private float timer;

        public DamageToCollisionOverTime(Damage damage, float timeBetweenTicks, bool tickImmediatly, D_Team targetTeam, D_EntityType targetType, params DeepVFXAction[] hitVFX)
        {
            this.damage = damage;
            this.timeBetweenTicks = timeBetweenTicks;
            this.tickImmediatly = tickImmediatly;
            this.targetTeam = targetTeam;
            this.targetType = targetType;
            this.vfxActions = hitVFX;
        }

        public override void InitializeBehavior()
        {
            if (tickImmediatly)
            {
                Tick();
            }
            parent.events.UpdateNorm += Update;
        }

        public override void DestroyBehavior()
        {
            parent.events.UpdateNorm -= Update;
        }

        private void Tick()
        {
            foreach (DeepEntity e in parent.activeCollisions.Values)
            {
                if (e.team == targetTeam && e.type == targetType)
                {
                    e.Hit(damage);
                    foreach (DeepVFXAction action in vfxActions)
                    {
                        action.Execute(e.cachedTransform.position);
                    }
                }
            }
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenTicks)
            {
                timer -= timeBetweenTicks;
                Tick();
            }
        }
    }
}