using UnityEngine;
using Sirenix.OdinInspector;

namespace DeepAction
{
    public class DeepEntityBuilder : MonoBehaviour
    {
        private const string baseEntityPath = "CubeEnemy";
        private GameObject _baseEntity;
        private GameObject baseEntity
        {
            get
            {
                if (_baseEntity == null)
                {
                    _baseEntity = Resources.Load(baseEntityPath) as GameObject;
                }

                return _baseEntity;
            }
        }

        public GameObject player;
        public GameObject enemy;

        void Start()
        {
            Player();
        }

        [Button]
        public DeepEntity CreateFromPrefab()
        {
            DeepEntity e = GameObject.Instantiate(baseEntity, DeepManager.instance.transform).GetComponent<DeepEntity>();
            e.Initialize(DeepEntityPresets.StaticBaseEntity);

            return e;
        }
        [Button]
        public DeepEntity Enemy()
        {
            DeepEntity e = GameObject.Instantiate(enemy, DeepManager.instance.transform).GetComponent<DeepEntity>();
            e.Initialize(DeepEntityPresets.ExampleEnemy());

            return e;
        }
        [Button]
        public DeepEntity Player()
        {
            DeepEntity e = GameObject.Instantiate(player, DeepManager.instance.transform).GetComponent<DeepEntity>();
            e.Initialize(DeepEntityPresets.ExamplePlayer());

            return e;
        }

        [Button]
        public DeepEntity CreateFromScratch()
        {
            GameObject g = new GameObject();
            g.transform.parent = DeepManager.instance.transform;
            DeepEntity e = g.AddComponent<DeepEntity>();
            e.Initialize(DeepEntityPresets.StaticBaseEntity);

            return e;
        }
    }
}
