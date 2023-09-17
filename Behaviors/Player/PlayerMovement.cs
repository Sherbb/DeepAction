using UnityEngine;

namespace DeepAction
{
    public class PlayerMovement : DeepBehavior
    {
        public override void InitializeBehavior()
        {
            parent.events.FixedUpdate += Move;
        }

        public override void DestroyBehavior()
        {
            parent.events.FixedUpdate -= Move;
        }

        void Move()
        {
            Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            Vector2 force = move * parent.attributes[D_Attribute.MoveSpeed].value;
            parent.mb.SetVelocity(Vector2.Lerp(Vector2.ClampMagnitude(parent.mb.velocity, parent.mb.effectiveVelocity.magnitude), force, 10f * Time.deltaTime));
        }
    }
}

