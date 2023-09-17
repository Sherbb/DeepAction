using UnityEngine;

namespace DeepAction
{
    public class PlayerAim : DeepBehavior
    {
        public override void InitializeBehavior()
        {
            parent.events.UpdateNorm += Aim;
        }

        public override void DestroyBehavior()
        {
            parent.events.UpdateNorm -= Aim;
        }

        RaycastHit hit;
        Ray ray;

        private void Aim()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layers.mouseLayerMask))
            {
                //! currently assuming 2d gameplay
                parent.SetAimDirection((new Vector3(hit.point.x, hit.point.y, parent.transform.position.z) - parent.transform.position).normalized);
            }
        }
    }
}
