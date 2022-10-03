using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction.Views
{
    public class RotateToMovementBodyVelocity : MonoBehaviour
    {
        public DeepMovementBody mb;
        public float rotateSpeed = 9f;

        void Update()
        {
            if (mb == null) return;
            if (mb.velocity.magnitude > 0f)
            {
                transform.up = Vector2.Lerp(transform.up, mb.velocity, Time.deltaTime * rotateSpeed);
            }
        }
    }
}
