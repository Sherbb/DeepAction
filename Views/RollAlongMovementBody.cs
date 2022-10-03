using UnityEngine;

namespace DeepAction.Views
{
    public class RollAlongMovementBody : MonoBehaviour
    {
        public DeepMovementBody mb;
        public Transform target;
        public float rotateSpeed;

        public Vector3 baseScale = Vector3.one;

        void Update()
        {
            if (mb == null)
            {
                return;
            }
            target.Rotate(new Vector3(mb.effectiveVelocity.y, mb.effectiveVelocity.x, 0f) * rotateSpeed * Time.deltaTime, Space.World);
        }
    }
}
