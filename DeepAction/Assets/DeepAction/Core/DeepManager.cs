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
        public List<DeepEntity> activeEntities = new List<DeepEntity>();

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
                foreach (DeepResource res in activeEntities[i].resources.Values)
                {
                    res.Tick();
                }
                if (activeEntities[i].resources[DeepEntity.damageHeirarchy[DeepEntity.damageHeirarchy.Length - 1]].GetValue() <= 0)
                {
                    activeEntities[i].Die();
                }
                activeEntities[i].events.Update?.Invoke();
            }
        }

        // Entites are killed during LATEUPDATE
        void LateUpdate()
        {
            for (int i = activeEntities.Count - 1; i >= 0; i--)
            {
                if (activeEntities[i].dying) activeEntities[i].gameObject.SetActive(false);
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