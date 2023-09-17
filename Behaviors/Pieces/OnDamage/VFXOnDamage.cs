using DeepAction.VFX;

namespace DeepAction
{
    public class VFXOnDamage : DeepBehavior
    {
        public DeepVFXAction[] vfxActions;

        public VFXOnDamage(params DeepVFXAction[] vfx)
        {
            vfxActions = vfx;
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
            foreach (DeepVFXAction a in vfxActions)
            {
                a.Execute(parent.transform.position);
            }
        }
    }
}
