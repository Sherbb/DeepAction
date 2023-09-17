using UnityEngine;
using DeepAction.VFX;

namespace DeepAction
{
    public class VFXOnBounce : DeepBehavior
    {
        public DeepVFXAction[] vfxActions;

        public VFXOnBounce(params DeepVFXAction[] vfx)
        {
            vfxActions = vfx;
        }

        public override void InitializeBehavior()
        {
            parent.events.OnBounce += DoVFX;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnBounce -= DoVFX;
        }

        private void DoVFX(Vector2 bouncePoint)
        {
            foreach (DeepVFXAction a in vfxActions)
            {
                a.Execute(bouncePoint);
            }
        }
    }
}
