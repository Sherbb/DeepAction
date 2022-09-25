using UnityEngine;

namespace DeepAction
{
    public class PlayerMovement : DeepBehavior
    {
        public override void IntitializeBehavior()
        {
            parent.events.FixedUpdate += Move;
        }

        public override void DestroyBehavior()
        {
            parent.events.FixedUpdate -= Move;
        }

        void Move()
        {
            Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector2 force = move * parent.attributes[D_Attribute.MoveSpeed].value;
            parent.rb.velocity = Vector2.Lerp(parent.rb.velocity, force, 10f * Time.deltaTime);
        }
    }
}

