using UnityEngine;

namespace DeepAction.Views
{
    public class RotateToEntityAim : MonoBehaviour
    {
        public DeepViewLink link;

        private void LateUpdate()
        {
            transform.right = link.entity.aimDirection;
        }
    }
}
