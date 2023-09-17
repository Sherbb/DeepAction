using DeepAction.VFX;
 
namespace DeepAction
{
    public class VFXOnLife : DeepBehavior
    {
        public DeepVFXAction[] vfxActions;

        public VFXOnLife(params DeepVFXAction[] vfx)
        {
            vfxActions = vfx;
        }

        public override void InitializeBehavior()
        {
            parent.events.OnEntityEnable += OnEnable;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnEntityEnable -= OnEnable;
        }

        private void OnEnable()
        {
            foreach (DeepVFXAction a in vfxActions)
            {
                a.Execute(parent.transform.position);
            }
        }
    }
}