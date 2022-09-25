using UnityEngine;

namespace DeepAction
{
    public class PlayerAim : DeepBehavior
    {
        public static LayerMask mouseLayerMask = 1 << 6;

        public override void IntitializeBehavior()
        {
            parent.events.Update += Aim;
        }

        public override void DestroyBehavior()
        {
            parent.events.Update -= Aim;
        }

        RaycastHit hit;
        Ray ray;

        private void Aim()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseLayerMask))
            {
                //! currently assuming 2d gameplay
                parent.SetAimDirection((new Vector3(hit.point.x, hit.point.y, parent.transform.position.z) - parent.transform.position).normalized);
            }
        }
    }
}
