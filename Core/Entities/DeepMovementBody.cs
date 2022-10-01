using UnityEngine;
using Sirenix.OdinInspector;

namespace DeepAction
{
    /// <summary>
    /// OPTIONAL. Can be substituted for simple rigidBody movement, or whatever you feel like.
    ///
    /// I created this so that I can move ALL entities the same way. Projectiles, players, enemies, etc.
    /// </summary>
    public class DeepMovementBody : MonoBehaviour
    {
        //does "movementBody" even make sense? lol

        //todo find a static place to put these layer masks....
        private static LayerMask _entityWallMask = 1 << 11;

        [HideInInspector]
        public DeepEntity entity;

        public enum CollisionType
        {
            Bounce,
            Slide,
        }

        [EnumToggleButtons]
        public CollisionType collisionType = CollisionType.Bounce;

        [ShowInInspector, ReadOnly]
        public Vector2 velocity { get; private set; }
        [ShowInInspector, ReadOnly]
        public Vector2 effectiveVelocity { get; private set; }

        private Vector2 _futurePosition;
        private RaycastHit2D[] _hits;
        private RaycastHit2D _hit;

        public void AddForce(Vector2 force)
        {
            velocity += force;
            velocity = Vector2.ClampMagnitude(velocity, entity.attributes[D_Attribute.MaxMoveSpeed].value);
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = Vector2.ClampMagnitude(velocity, entity.attributes[D_Attribute.MaxMoveSpeed].value);
        }

        //todo take this off update and call it through entity manager
        public void Update()
        {
            //drag
            velocity = velocity * (1f - Time.deltaTime * entity.attributes[D_Attribute.Drag].value);

            switch (collisionType)
            {
                case CollisionType.Bounce:
                    BounceCollision();
                    effectiveVelocity = velocity;
                    break;
                case CollisionType.Slide:
                    Vector3 startPos = transform.position;
                    if (SlideCollision())
                    {
                        //apply slide friction
                        velocity = velocity * (1f - Time.deltaTime * entity.attributes[D_Attribute.SlideFriction].value);
                    }
                    effectiveVelocity = (transform.position - startPos) * (1f / Time.deltaTime);
                    break;
            }

        }

        private void BounceCollision()
        {
            float frameDistance = velocity.magnitude * Time.deltaTime;
            do
            {
                //todo nonAlloc
                _hits = Physics2D.CircleCastAll(transform.position, entity.attributes[D_Attribute.MovementRadius].value, velocity.normalized, frameDistance, _entityWallMask);

                if (_hits.Length == 0)
                {
                    transform.position = (Vector2)transform.position + (velocity.normalized * frameDistance);
                    return;
                }

                System.Array.Sort(_hits, (x, y) => x.distance.CompareTo(y.distance));
                _hit = _hits[0];

                if (_hit.point == Vector2.zero)
                {
                    _hit.point = transform.position;
                }

                frameDistance -= _hit.distance;
                transform.position = _hit.point + (_hit.normal * (entity.attributes[D_Attribute.MovementRadius].value * 1.015f));
                entity.events.OnBounce?.Invoke(transform.position);
                velocity = Vector2.Reflect(velocity, _hit.normal);
                velocity *= entity.attributes[D_Attribute.Bounciness].value;
                frameDistance *= entity.attributes[D_Attribute.Bounciness].value;

            } while (frameDistance > 0f);

        }

        private bool SlideCollision()
        {
            float frameDistance = velocity.magnitude * Time.deltaTime;
            Vector2 slideDir = velocity.normalized;
            bool slideThisFrame = false;
            do
            {
                //todo nonaAlloc
                _hits = Physics2D.CircleCastAll(transform.position, entity.attributes[D_Attribute.MovementRadius].value, slideDir, frameDistance, _entityWallMask);

                if (_hits.Length == 0)
                {
                    transform.position = (Vector2)transform.position + (slideDir * frameDistance);
                    return slideThisFrame;
                }

                slideThisFrame = true;

                System.Array.Sort(_hits, (x, y) => x.distance.CompareTo(y.distance));
                _hit = _hits[0];

                if (_hit.point == Vector2.zero)
                {
                    _hit.point = transform.position;
                }

                frameDistance -= _hit.distance;

                transform.position = _hit.point + (_hit.normal * (entity.attributes[D_Attribute.MovementRadius].value * 1.025f));

                slideDir = (Vector2.Reflect(velocity.normalized, _hit.normal).normalized + velocity.normalized).normalized;

                //find the remaining distance by projecting onto the new slideDirection
                Vector2 p = Vector3.Project(velocity.normalized * frameDistance, slideDir);
                frameDistance = p.magnitude;

            } while (frameDistance > 0f);

            return slideThisFrame;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, entity.attributes[D_Attribute.MovementRadius].value);
        }
    }
}