using UnityEngine;

namespace DeepAction
{
    public class DieOnBounce : DeepBehavior
    {
        public override void InitializeBehavior()
        {
            parent.events.OnBounce += Die;
        }
        public override void DestroyBehavior()
        {
            parent.events.OnBounce -= Die;
        }

        private void Die(Vector2 foo)
        {
            parent.Die();
        }
    }
}
