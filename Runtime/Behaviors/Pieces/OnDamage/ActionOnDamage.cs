using System;

namespace DeepAction
{
    public class ActionOnDamage : DeepBehavior
    {
        private Action<DeepEntity, float>[] actions;
        public ActionOnDamage(params Action<DeepEntity, float>[] actions)
        {
            this.actions = actions;
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
            foreach (Action<DeepEntity, float> a in actions)
            {
                a.Invoke(parent, damage);
            }
        }
    }
}
