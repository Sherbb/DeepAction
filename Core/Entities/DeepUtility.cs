using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DeepAction
{
    /// <summary>
    /// Contains lots of usefull static gameplay actions.
    /// </summary>
    public class DeepUtility
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

        public static bool AreaBehavior(Vector2 position, float radius, DeepBehavior behavior, DeepEntity owner, D_Team targetTeam, bool applyDublicates = false)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius, enityLayerMask);
            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent(out DeepEntity entity) && entity.team == targetTeam && (applyDublicates || !entity.HasBehavior(behavior.GetType())))
                {
                    entity.AddBehavior(behavior.Clone(), owner);
                }
            }
            return hits.Length > 0;
        }

        public static bool AreaDamage(Vector2 position, float radius, Damage damage, DeepEntity owner, D_Team targetTeam)
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

        public static DeepEntity[] GetEntitiesInArea(Vector2 position, float radius, D_EntityType type)//todo add type/team array like below
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius, enityLayerMask);
            List<DeepEntity> entities = new List<DeepEntity>();
            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent(out DeepEntity entity) && entity.type == type)
                {
                    entities.Add(entity);
                }
            }
            return entities.ToArray();
        }

        //! THIS IT NOT NON ALLOC FIX THIS LOL.
        public static int GetEntitiesInAreaNonAlloc(Vector2 position, float radius, DeepEntity[] buffer, D_EntityType[] type, D_Team[] team)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius, enityLayerMask);
            int count = 0;
            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent(out DeepEntity entity) && type.Contains(entity.type) && team.Contains(entity.team))
                {
                    buffer[count] = entity;
                    count++;
                    if (count >= buffer.Length)
                    {
                        return count;
                    }
                }
            }
            return count;
        }
    }
}
