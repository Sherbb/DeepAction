using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction.Views
{
    public class RotateToRigidbodyVelocity : MonoBehaviour
    {
        public Rigidbody2D rb;
        public float rotateSpeed = 9f;

        void Update()
        {
            if (rb == null) return;
            if (rb.velocity.magnitude > 0f)
            {
                transform.up = Vector2.Lerp(transform.up, rb.velocity, Time.deltaTime * rotateSpeed);
            }
        }
    }
}
