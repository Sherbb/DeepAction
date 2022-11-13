using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Newtonsoft.Json;

namespace DeepAction
{
    [DefaultExecutionOrder(-400)]
    public class DeepManager : MonoBehaviour
    {
        public static DeepManager instance { get; private set; }
        private S_Game game => App.state.game;

        public List<DeepEntity> baseEntityPool { get; private set; } = new List<DeepEntity>();
        private Transform inactiveEntityParent;
        private Transform activeEntityParent;

        void Awake()
        {
            instance = this;
            GameObject g = new GameObject();
            g.name = "InactiveEntities";
            g.transform.parent = transform;
            inactiveEntityParent = g.transform;

            GameObject gg = new GameObject();
            gg.name = "ActiveEntities";
            gg.transform.parent = transform;
            activeEntityParent = gg.transform;

            for (int i = 0; i < 100; i++)
            {
                CreateBaseEntity();
            }
        }

        private void CreateBaseEntity()
        {
            GameObject g = new GameObject();
            g.name = "Entity";
            g.layer = 10;
            g.SetActive(false);
            g.AddComponent<CircleCollider2D>().isTrigger = true;
            var rb = g.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.freezeRotation = true;
            g.AddComponent<DeepMovementBody>();
            g.transform.parent = inactiveEntityParent;
            baseEntityPool.Add(g.AddComponent<DeepEntity>());
        }

        public DeepEntity PullEntity()
        {
            if (baseEntityPool.Count <= 0)
            {
                CreateBaseEntity();
            }
            DeepEntity e = baseEntityPool[0];
            baseEntityPool.RemoveAt(0);
            e.transform.parent = activeEntityParent;
            return e;
        }

        public void ReturnEntity(DeepEntity e)
        {
            e.transform.parent = inactiveEntityParent;
            baseEntityPool.Add(e);
            e.gameObject.SetActive(false);
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
                DeepEntity entity = game.activeEntities[i];
                if (entity.dying)
                {
                    for (int j = entity.behaviors.Count - 1; j >= 0; j--)
                    {
                        entity.RemoveBehavior(entity.behaviors[j]);
                    }

                    //remove any lingering views
                    for (int j = entity.views.Count - 1; j >= 0; j--)
                    {
                        //views are removed from the list inside StartReturn()
                        entity.views[j].StartReturn();
                    }

                    App.state.game.DeregisterEntity(entity);
                    entity.gameObject.SetActive(false);
                    ReturnEntity(entity);
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