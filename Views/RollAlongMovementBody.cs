using UnityEngine;

namespace DeepAction.Views
{
    public class RollAlongMovementBody : MonoBehaviour
    {
        public DeepViewLink link;
        public Transform target;
        public float rotateSpeed;

        public Vector3 baseScale = Vector3.one;

        void Update()
        {
            if (link.entity == null)
            {
                return;
            }
            target.Rotate(new Vector3(link.entity.mb.effectiveVelocity.y, link.entity.mb.effectiveVelocity.x, 0f) * rotateSpeed * Time.deltaTime, Space.World);
        }
    }
}
