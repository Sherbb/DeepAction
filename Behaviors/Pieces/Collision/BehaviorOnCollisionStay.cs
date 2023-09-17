using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    /// <summary>
    /// Apply behavior to all entities that collide with parent entity. Filter based on team and type. Behavior is always applied so things will stack.
    /// </summary>
    public class BehaviorOnCollisionStay : DeepBehavior
    {
        public DeepBehavior behavior;
        public D_TeamSelector teamTarget;
        public D_EntityTypeSelector typeTarget;

        private HashSet<Tuple<DeepEntity, DeepBehavior>> containedEntities = new HashSet<Tuple<DeepEntity, DeepBehavior>>();

        public BehaviorOnCollisionStay(DeepBehavior behavior, D_TeamSelector teamTarget = D_TeamSelector.All, D_EntityTypeSelector typeTarget = D_EntityTypeSelector.All)
        {
            this.behavior = behavior;
            this.teamTarget = teamTarget;
            this.typeTarget = typeTarget;
        }

        public override void InitializeBehavior()
        {
            parent.events.OnEntityCollisionEnter += EntityEnter;
            parent.events.OnEntityCollisionExit += EntityExit;
        }

        public override void DestroyBehavior()
        {
            foreach (Tuple<DeepEntity, DeepBehavior> pair in containedEntities)
            {
                pair.Item1.RemoveBehavior(pair.Item2);
            }

            parent.events.OnEntityCollisionEnter -= EntityEnter;
            parent.events.OnEntityCollisionExit -= EntityExit;
        }

        private void EntityEnter(DeepEntity e)
        {
            if (!teamTarget.HasTeam(e.team) || !typeTarget.HasEntityType(e.type))
            {
                return;
            }
            containedEntities.Add(new Tuple<DeepEntity, DeepBehavior>(e, e.AddBehavior(behavior.Clone(), owner)));
        }

        private void EntityExit(DeepEntity e)
        {
            var found = containedEntities.FirstOrDefault(pair => pair.Item1 == e);

            if (found != null)
            {
                found.Item1.RemoveBehavior(found.Item2);
                containedEntities.Remove(found);
            }
        }
    }
}