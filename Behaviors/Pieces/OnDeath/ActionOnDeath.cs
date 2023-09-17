using System;

namespace DeepAction
{
    public class ActionOnDeath : DeepBehavior
    {
        private Action<DeepEntity>[] actions;
        public ActionOnDeath(params Action<DeepEntity>[] actions)
        {
            this.actions = actions;
        }

        public override void InitializeBehavior()
        {
            parent.events.OnEntityDie += OnDie;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnEntityDie -= OnDie;
        }

        private void OnDie()
        {
            foreach (Action<DeepEntity> a in actions)
            {
                a.Invoke(parent);
            }
        }
    }
}