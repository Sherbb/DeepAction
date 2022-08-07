using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DeepAction
{
    public class DeepManager : MonoBehaviour
    {
        //Every DeepEntity adds itself to the manager
        //Gives us a nice place to iterate over entities while being a tiny bit more performant.

        public static DeepManager instance;

        [ReadOnly]
        public List<DeepEntity> activeEntities = new List<DeepEntity>();//todo make array

        void Awake()
        {
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

        // Entites are killed during LATEUPDATE
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