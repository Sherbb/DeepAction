using UnityEngine;

namespace DeepAction.Views
{
    public class RollAlongMovementBody : MonoBehaviour
    {
        public DeepViewLink link;
        public Transform target;
        public float rotateSpeed;

        private Vector3 velocity;

        void Start()
        {
            if (link.entity == null)
            {
                Debug.LogError("RollAlongMovementBody has null link");
                enabled = false;
            }
        }

        void Update()
        {
            velocity = link.entity.mb.effectiveVelocity;
            target.rotation = target.rotation * Quaternion.Euler(new Vector3(velocity.y, velocity.x, 0f) * rotateSpeed * Time.deltaTime);
        }
    }
}
