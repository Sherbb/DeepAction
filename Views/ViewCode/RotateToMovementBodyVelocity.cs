using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction.Views
{
    public class RotateToMovementBodyVelocity : MonoBehaviour
    {
        public DeepViewLink link;
        public float rotateSpeed = 9f;

        void Update()
        {
            if (link.entity == null) return;
            if (link.entity.mb.velocity.magnitude > 0f)
            {
                transform.up = Vector2.Lerp(transform.up, link.entity.mb.velocity, Time.deltaTime * rotateSpeed);
            }
        }
    }
}
