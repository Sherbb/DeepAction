using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class MoveTowardsPlayer : DeepBehavior
    {
        private DeepMovementBody mb;
        private float moveSpeed;

        public override void InitializeBehavior()
        {
            mb = parent.mb;
            parent.events.FixedUpdate += Move;
            moveSpeed = parent.attributes[D_Attribute.MoveSpeed].value;
            parent.attributes[D_Attribute.MoveSpeed].onValueChanged += UpdateMoveSpeed;
        }

        public override void DestroyBehavior()
        {
            parent.attributes[D_Attribute.MoveSpeed].onValueChanged -= UpdateMoveSpeed;
            parent.events.FixedUpdate -= Move;
        }

        private void UpdateMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }

        private void Move()
        {
            Vector2 move = App.state.game.playerPosition.value - parent.cachedTransform.position;
            Vector2 force = (move / Mathf.Sqrt(move.sqrMagnitude)) * moveSpeed;
            mb.SetVelocity(Vector2.Lerp(Vector2.ClampMagnitude(mb.velocity, mb.effectiveVelocity.magnitude), force, 5f * Time.deltaTime));
        }
    }
}

