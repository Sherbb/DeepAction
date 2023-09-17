using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class DamageOverTime : DeepBehavior
    {
        public Damage damage;
        public int totalTicks;
        public float timeBetweenTicks;
        public bool tickImmediatly;
        public string[] views;

        public DeepViewLink[] viewLinks;
        private float timer;
        private int ticks = 0;

        public DamageOverTime(Damage damage, int totalTicks, float timeBetweenTicks, bool tickImmediatly, params string[] views)
        {
            this.damage = damage;
            this.totalTicks = totalTicks;
            this.timeBetweenTicks = timeBetweenTicks;
            this.tickImmediatly = tickImmediatly;
            this.views = views;
        }

        public override void InitializeBehavior()
        {
            for (int i = 0; i < views.Length; i++)
            {
                viewLinks = new DeepViewLink[views.Length];
                viewLinks[i] = parent.AddView(views[i]);
            }

            if (tickImmediatly)
            {
                Tick();
            }
            parent.events.UpdateNorm += Update;
        }

        public override void DestroyBehavior()
        {
            parent.events.UpdateNorm -= Update;
            for (int i = 0; i < viewLinks.Length; i++)
            {
                viewLinks[i]?.StartReturn();
            }
        }

        private void Tick()
        {
            parent.Hit(owner, damage);
            ticks++;

            if (ticks >= totalTicks)
            {
                parent.RemoveBehavior(this);
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