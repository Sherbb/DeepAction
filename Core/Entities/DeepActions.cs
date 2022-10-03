using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    /// <summary>
    /// Contains lots of usefull static gameplay actions.
    /// </summary>
    public class DeepActions
    {
        public static LayerMask enityLayerMask = 1 << 10;

        public static bool AreaImpulse(Vector2 position, float radius, float force, D_Team targetTeam)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius, enityLayerMask);
            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent(out DeepEntity entity) && entity.team == targetTeam)
                {
                    entity.mb.AddForce(((Vector2)entity.transform.position - position).normalized * force);
                }
            }
            return hits.Length > 0;
        }

        public static bool AreaBehavior(Vector2 position, float radius, DeepBehavior behavior, D_Team targetTeam)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius, enityLayerMask);
            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent(out DeepEntity entity) && entity.team == targetTeam)
                {
                    entity.AddBehavior(behavior.Clone());
                }
            }
            return hits.Length > 0;
        }

        public static bool AreaDamage(Vector2 position, float radius, Damage damage, D_Team targetTeam)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius, enityLayerMask);
            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent(out DeepEntity entity) && entity.team == targetTeam)
                {
                    entity.Hit(damage);
                }
            }
            return hits.Length > 0;
        }
    }
}
