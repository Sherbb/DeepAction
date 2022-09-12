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
        public static DeepManager instance { get; private set; }
        private S_Game game => App.state.game;

        void Awake()
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // All entity logic runs during UPDATE
        void Update()
        {
            for (int i = game.activeEntities.list.Count - 1; i >= 0; i--)
            {
                game.activeEntities.list[i].events.Update?.Invoke();
            }
        }

        // Entites are killed (disabled) during LATEUPDATE
        void LateUpdate()
        {
            for (int i = game.activeEntities.list.Count - 1; i >= 0; i--)
            {
                if (game.activeEntities.list[i].dying)
                {
                    //Remove all behaviors with [RemoveOnDeath] flag
                    for (int j = game.activeEntities.list[i].behaviors.Count - 1; j >= 0; j--)
                    {
                        if (game.activeEntities.list[i].behaviors[j].removeOnDeath)
                        {
                            game.activeEntities.list[i].RemoveBehavior(game.activeEntities.list[i].behaviors[j]);
                        }
                    }
                    game.activeEntities.list[i].gameObject.SetActive(false);
                }
            }
        }

        void FixedUpdate()
        {
            for (int i = game.activeEntities.list.Count - 1; i >= 0; i--)
            {
                game.activeEntities.list[i].events.FixedUpdate?.Invoke();
            }
        }
    }
}