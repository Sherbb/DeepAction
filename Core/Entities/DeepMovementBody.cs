using UnityEngine;
using Unity.Jobs;

namespace DeepAction
{
    // Can be substituted for simple rigidBody movement, or whatever you feel like.
    // I created this so that I can move ALL entities the same way. Projectiles, players, enemies, etc.
    public class DeepMovementBody : MonoBehaviour
    {
        // if you have really fast projectiles, or lots of overlapping walls consider raising this.
        private static int raycastPoolSize = 5;

        [HideInInspector]
        public DeepEntity entity;

        public enum CollisionType
        {
            Bounce,
            Slide,
        }

        public CollisionType collisionType = CollisionType.Bounce;

        public Vector2 velocity { get; private set; }
        public Vector2 effectiveVelocity { get; private set; }

        private Vector2 _futurePosition;
        private RaycastHit2D _hit;
        private RaycastHit2D[] _hitpool = new RaycastHit2D[raycastPoolSize];

        private void OnEnable()
        {
            DeepUpdate.UpdateEarly += UpdateEarly;
        }

        private void OnDisable()
        {
            DeepUpdate.UpdateEarly -= UpdateEarly;
        }

        public void AddForce(Vector2 force)
        {
            velocity += force;
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        public void UpdateEarly()
        {
            //drag
            velocity = velocity * (1f - Time.deltaTime * entity.attributes[D_Attribute.Drag].value);
            if (velocity.magnitude < .010f)
            {
                return;
            }

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
                    effectiveVelocity = (entity.cachedTransform.position - startPos) * (1f / Time.deltaTime);
                    break;
            }

        }

        private void BounceCollision()
        {
            float frameDistance = velocity.magnitude * Time.deltaTime;
            Vector2 velocityNormalized = velocity / Mathf.Sqrt(velocity.sqrMagnitude);
            do
            {
                int hits = Physics2D.CircleCastNonAlloc(entity.cachedTransform.position, entity.attributes[D_Attribute.MovementRadius].value, velocityNormalized, _hitpool, frameDistance, Layers.entityWall);

                if (hits == 0)
                {
                    entity.cachedTransform.position = (Vector2)entity.cachedTransform.position + (velocityNormalized * frameDistance);
                    return;
                }

                float closestDist = Mathf.Infinity;
                for (int i = 0; i < hits; i++)
                {
                    if (_hitpool[i].distance < closestDist)
                    {
                        _hit = _hitpool[i];
                        closestDist = _hitpool[i].distance;
                    }
                }

                if (_hit.point == Vector2.zero)
                {
                    _hit.point = transform.position;
                }

                frameDistance -= _hit.distance;
                entity.cachedTransform.position = _hit.point + (_hit.normal * (entity.attributes[D_Attribute.MovementRadius].value * 1.015f));
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
                int hits = Physics2D.CircleCastNonAlloc(entity.cachedTransform.position, entity.attributes[D_Attribute.MovementRadius].value, slideDir, _hitpool, frameDistance, Layers.entityWall);

                if (hits == 0)
                {
                    entity.cachedTransform.position = (Vector2)entity.cachedTransform.position + (slideDir * frameDistance);
                    return slideThisFrame;
                }

                slideThisFrame = true;

                float closestDist = Mathf.Infinity;
                for (int i = 0; i < hits; i++)
                {
                    if (_hitpool[i].distance < closestDist)
                    {
                        _hit = _hitpool[i];
                        closestDist = _hitpool[i].distance;
                    }
                }

                if (_hit.point == Vector2.zero)
                {
                    _hit.point = entity.cachedTransform.position;
                }

                frameDistance -= _hit.distance;

                entity.cachedTransform.position = _hit.point + (_hit.normal * (entity.attributes[D_Attribute.MovementRadius].value * 1.025f));

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