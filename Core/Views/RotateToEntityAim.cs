using UnityEngine;

namespace DeepAction.Views
{
    public class RotateToEntityAim : MonoBehaviour
    {
        public DeepEntity entity;

        private void LateUpdate()
        {
            transform.right = entity.aimDirection;
        }
    }
}
