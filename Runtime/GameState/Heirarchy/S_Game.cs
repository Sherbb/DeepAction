using System;
using System.Linq;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace DeepAction
{
    [Serializable]
    public class S_Game
    {
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public DeepStateList<DeepEntity> activeEntities { get; private set; } = new DeepStateList<DeepEntity>();

        // * Lookups
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public Dictionary<D_EntityType, DeepStateList<DeepEntity>> entityByTypeLookup { get; private set; } = new Dictionary<D_EntityType, DeepStateList<DeepEntity>>();
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public Dictionary<D_Team, DeepStateList<DeepEntity>> entityByTeamLookup { get; private set; } = new Dictionary<D_Team, DeepStateList<DeepEntity>>();
        public DeepEntity playerMain { get; private set; }
        public DeepState<Vector3> playerPosition { get; private set; } = new DeepState<Vector3>(Vector3.zero);

        public DeepState<DeepEntity> closestEnemyActor { get; private set; } = new DeepState<DeepEntity>(null);

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public Dictionary<D_Team, Dictionary<D_EntityType, DeepStateList<DeepEntity>>> entityByTeamAndTypeLookup { get; private set; } = new Dictionary<D_Team, Dictionary<D_EntityType, DeepStateList<DeepEntity>>>();

        public S_GameArtifacts artifacts = new();

        public S_Game()
        {
            foreach (D_Team team in Enum.GetValues(typeof(D_Team)))
            {
                entityByTeamLookup.Add(team, new DeepStateList<DeepEntity>());
                entityByTeamAndTypeLookup.Add(team, new Dictionary<D_EntityType, DeepStateList<DeepEntity>>());
            }
            foreach (D_EntityType type in Enum.GetValues(typeof(D_EntityType)))
            {
                entityByTypeLookup.Add(type, new DeepStateList<DeepEntity>());
                foreach (D_Team team in Enum.GetValues(typeof(D_Team)))
                {
                    entityByTeamAndTypeLookup[team].Add(type, new DeepStateList<DeepEntity>());
                }
            }
            activeEntities = new DeepStateList<DeepEntity>();

            DeepUpdate.UpdateEarly += UpdatePlayerState;
        }

        public void RegisterEntity(DeepEntity e)
        {
            //entities are added to global state.
            App.state.game.activeEntities.Add(e);
            App.state.game.entityByTypeLookup[e.type].Add(e);
            App.state.game.entityByTeamLookup[e.team].Add(e);
            App.state.game.entityByTeamAndTypeLookup[e.team][e.type].Add(e);
        }

        public void DeregisterEntity(DeepEntity e)
        {
            App.state.game.activeEntities.Remove(e);
            App.state.game.entityByTypeLookup[e.type].Remove(e);
            App.state.game.entityByTeamLookup[e.team].Remove(e);
            App.state.game.entityByTeamAndTypeLookup[e.team][e.type].Remove(e);
        }

        //////////////// * UPDATES

        private void UpdatePlayerState()
        {
            playerMain = entityByTeamAndTypeLookup[D_Team.Player][D_EntityType.Actor].First();
            playerPosition.SetValue(playerMain == null ? Vector3.zero : playerMain.transform.position);

            //Find the closest ENEMY to the player
            DeepEntity candidate = null;
            float lastDistance = Mathf.Infinity;
            float distance;
            foreach (DeepEntity e in entityByTeamAndTypeLookup[D_Team.Enemy][D_EntityType.Actor])
            {
                distance = (e.cachedTransform.position - playerPosition.value).sqrMagnitude;
                if (candidate == null || distance < lastDistance)
                {
                    candidate = e;
                    lastDistance = distance;
                }
            }
            closestEnemyActor.SetValue(candidate);
        }
    }
}
