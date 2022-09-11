using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Newtonsoft.Json;

namespace DeepAction
{
    public class DeepManager : MonoBehaviour
    {
        //Every DeepEntity adds itself to the manager
        //Gives us a nice place to iterate and organize entities while being a tiny bit more performant.

        public static DeepManager instance { get; private set; }

        [ReadOnly]
        [ShowInInspector]
        public List<DeepEntity> activeEntities { get; private set; }

        // * Lookups
        [ShowInInspector]
        public Dictionary<D_EntityType, List<DeepEntity>> entityByTypeLookup { get; private set; }
        [ShowInInspector]
        public Dictionary<D_Team, List<DeepEntity>> entityByTeamLookup { get; private set; }
        /// <summary>
        /// Sort entities by team, then by type. example of getting all player actors:
        /// 
        /// entityByTeamAndTypeLookup[D_Team.Player][D_EntityType.Actor].Count;
        /// </summary>
        [ShowInInspector]
        public Dictionary<D_Team, Dictionary<D_EntityType, List<DeepEntity>>> entityByTeamAndTypeLookup { get; private set; }

        public void RegisterEntity(DeepEntity e)
        {
            activeEntities.Add(e);
            entityByTypeLookup[e.type].Add(e);
            entityByTeamLookup[e.team].Add(e);
            entityByTeamAndTypeLookup[e.team][e.type].Add(e);

            App.state.game.activeEntities.Add(e);
            App.state.game.entityByTypeLookup[e.type].Add(e);
            App.state.game.entityByTeamLookup[e.team].Add(e);
            App.state.game.entityByTeamAndTypeLookup[e.team][e.type].Add(e);
        }

        public void DeregisterEntity(DeepEntity e)
        {
            activeEntities.Remove(e);
            entityByTypeLookup[e.type].Remove(e);
            entityByTeamLookup[e.team].Remove(e);
            entityByTeamAndTypeLookup[e.team][e.type].Remove(e);
        }

        void Awake()
        {
            //initialize lookups
            entityByTeamLookup = new Dictionary<D_Team, List<DeepEntity>>();
            entityByTypeLookup = new Dictionary<D_EntityType, List<DeepEntity>>();
            entityByTeamAndTypeLookup = new Dictionary<D_Team, Dictionary<D_EntityType, List<DeepEntity>>>();
            foreach (D_Team team in Enum.GetValues(typeof(D_Team)))
            {
                entityByTeamLookup.Add(team, new List<DeepEntity>());
                entityByTeamAndTypeLookup.Add(team, new Dictionary<D_EntityType, List<DeepEntity>>());
            }
            foreach (D_EntityType type in Enum.GetValues(typeof(D_EntityType)))
            {
                entityByTypeLookup.Add(type, new List<DeepEntity>());
                foreach (D_Team team in Enum.GetValues(typeof(D_Team)))
                {
                    entityByTeamAndTypeLookup[team].Add(type, new List<DeepEntity>());
                }
            }

            activeEntities = new List<DeepEntity>();
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // All entity logic runs during UPDATE
        void Update()
        {
            for (int i = activeEntities.Count - 1; i >= 0; i--)
            {
                activeEntities[i].events.Update?.Invoke();
            }
        }

        // Entites are killed (disabled) during LATEUPDATE
        void LateUpdate()
        {
            for (int i = activeEntities.Count - 1; i >= 0; i--)
            {
                if (activeEntities[i].dying)
                {
                    //Remove all behaviors with [RemoveOnDeath] flag
                    for (int j = activeEntities[i].behaviors.Count - 1; j >= 0; j--)
                    {
                        if (activeEntities[i].behaviors[j].removeOnDeath) activeEntities[i].RemoveBehavior(activeEntities[i].behaviors[j]);
                    }
                    activeEntities[i].gameObject.SetActive(false);
                }
            }
        }

        void FixedUpdate()
        {
            for (int i = activeEntities.Count - 1; i >= 0; i--)
            {
                activeEntities[i].events.FixedUpdate?.Invoke();
            }
        }
    }
}