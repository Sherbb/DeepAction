using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        void Start()
        {
            DeepUpdate.UpdateEarly += UpdateEarly;
            DeepUpdate.UpdateSchedule += UpdateSchedule;
            DeepUpdate.UpdateNorm += UpdateNorm;
            DeepUpdate.UpdateComplete += UpdateComplete;
            DeepUpdate.UpdateFinal += UpdateFinal;

            DeepUpdate.LateNorm += LateNorm;
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
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
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

        private void UpdateEarly()
        {
            for (int i = game.activeEntities.list.Count - 1; i >= 0; i--)
            {
                game.activeEntities[i].events.UpdateEarly?.Invoke();
            }
        }

        private void UpdateSchedule()
        {
            for (int i = game.activeEntities.list.Count - 1; i >= 0; i--)
            {
                game.activeEntities[i].events.UpdateSchedule?.Invoke();
            }
        }

        private void UpdateNorm()
        {
            for (int i = game.activeEntities.list.Count - 1; i >= 0; i--)
            {
                if (game.activeEntities[i].activeCollisions.Count > 0)
                {
                    game.activeEntities[i].events.OnEntityCollisionStay?.Invoke();
                }
            }

            for (int i = game.activeEntities.list.Count - 1; i >= 0; i--)
            {
                game.activeEntities[i].events.UpdateNorm?.Invoke();
            }
        }

        private void UpdateComplete()
        {
            foreach (DeepEntity e in game.activeEntities)
            {
                e.events.UpdateComplete?.Invoke();
            }
        }
         
        private void UpdateFinal()
        {
            for (int i = game.activeEntities.list.Count - 1; i >= 0; i--)
            {
                game.activeEntities[i].events.UpdateFinal?.Invoke();
            }
        }

        // Entites are killed (disabled) during LATEUPDATE-norm
        private void LateNorm()
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

        //todo use DeepUpdate
        void FixedUpdate()
        {
            for (int i = game.activeEntities.list.Count - 1; i >= 0; i--)
            {
                game.activeEntities[i].events.FixedUpdate?.Invoke();
            }
        }
    }
}