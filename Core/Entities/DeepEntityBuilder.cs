using UnityEngine;
using Sirenix.OdinInspector;

namespace DeepAction
{
    //example script
    public class DeepEntityBuilder : MonoBehaviour
    {

        public float spawnPerSec = 0f;
        private float spawnTimer;

        void Start()
        {
            DeepEntity.Create(DeepEntityPresets.ExamplePlayer(), new Vector2(0f, -20f), Quaternion.identity, "PlayerView");
        }

        void Update()
        {
            spawnTimer += Time.deltaTime * spawnPerSec;
            if (spawnTimer >= 1f)
            {
                DeepEntity.Create(DeepEntityPresets.ExampleEnemy(), Vector2.zero, Quaternion.identity, "CubeEnemyView");
                spawnTimer -= 1f;
            }
        }
    }
}
