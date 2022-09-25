using UnityEngine;

namespace DeepAction.Views
{
    public class RollAlongRigidbody : MonoBehaviour
    {
        public Rigidbody2D rb;
        public Transform target;
        public float rotateSpeed;

        public Vector3 baseScale = Vector3.one;
        private float rndTime;

        private void Start()
        {
            rndTime = Random.Range(0, 25f);
        }

        void Update()
        {
            if (rb == null) return;
            target.Rotate(new Vector3(rb.velocity.y, rb.velocity.x, 0f) * rotateSpeed * Time.deltaTime, Space.World);
        }
    }
}
