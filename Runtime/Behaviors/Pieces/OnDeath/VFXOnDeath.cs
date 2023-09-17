using DeepAction.VFX;

namespace DeepAction
{
    public class VFXOnDeath : DeepBehavior
    {
        public DeepVFXAction[] vfxActions;

        public VFXOnDeath(params DeepVFXAction[] vfx)
        {
            vfxActions = vfx;
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
            foreach (DeepVFXAction a in vfxActions)
            {
                a.Execute(parent.transform.position);
            }
        }
    }
}