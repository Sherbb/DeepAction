using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DeepAction
{
    // Read notation like:
    // S_Game => "GameState
    [Serializable]
    public class S_App
    {
        [ShowInInspector]
        public S_Game game { get; private set; } = new S_Game();
        [ShowInInspector]
        public S_UI ui { get; private set; } = new S_UI();
    }

    [Serializable]
    public class S_Game
    {
        [ShowInInspector]
        public DeepStateList<DeepEntity> activeEntities { get; private set; } = new DeepStateList<DeepEntity>();

        // * Lookups
        [ShowInInspector]
        public Dictionary<D_EntityType, DeepStateList<DeepEntity>> entityByTypeLookup { get; private set; } = new Dictionary<D_EntityType, DeepStateList<DeepEntity>>();
        [ShowInInspector]
        public Dictionary<D_Team, DeepStateList<DeepEntity>> entityByTeamLookup { get; private set; } = new Dictionary<D_Team, DeepStateList<DeepEntity>>();
        /// <summary>
        /// Sort entities by team, then by type. example of getting all player actors:
        /// 
        /// entityByTeamAndTypeLookup[D_Team.Player][D_EntityType.Actor].Count;
        /// </summary>
        [ShowInInspector]
        public Dictionary<D_Team, Dictionary<D_EntityType, DeepStateList<DeepEntity>>> entityByTeamAndTypeLookup { get; private set; } = new Dictionary<D_Team, Dictionary<D_EntityType, DeepStateList<DeepEntity>>>();

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
        }

    }

    [Serializable]
    public class S_UI
    {

    }
}
