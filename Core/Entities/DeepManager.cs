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
                game.activeEntities[i].events.Update?.Invoke();
            }

            for (int i = game.activeEntities.list.Count - 1; i >= 0; i--)
            {
                game.activeEntities[i].CheckCollisionStay();
            }
        }

        // Entites are killed (disabled) during LATEUPDATE
        void LateUpdate()
        {
            for (int i = game.activeEntities.list.Count - 1; i >= 0; i--)
            {
                if (game.activeEntities[i].dying)
                {
                    //Remove all behaviors with [RemoveOnDeath] flag
                    for (int j = game.activeEntities[i].behaviors.Count - 1; j >= 0; j--)
                    {
                        if (game.activeEntities[i].behaviors[j].removeOnDeath)
                        {
                            game.activeEntities[i].RemoveBehavior(game.activeEntities[i].behaviors[j]);
                        }
                    }
                    game.activeEntities[i].gameObject.SetActive(false);
                }
            }
        }

        void FixedUpdate()
        {
            for (int i = game.activeEntities.list.Count - 1; i >= 0; i--)
            {
                game.activeEntities[i].events.FixedUpdate?.Invoke();
            }
        }
    }
}