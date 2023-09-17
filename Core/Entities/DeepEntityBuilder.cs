using UnityEngine;

namespace DeepAction
{
    //example script
    public class DeepEntityBuilder : MonoBehaviour
    {
        public float spawnPerSec = 0f;
        private float spawnTimer;

        void Start()
        {
            DeepEntity.Create(T_Player.BasicPlayer(), new Vector2(0f, -20f), Quaternion.identity, "PlayerView");
        }

        void Update()
        {
            if (!Input.GetKey(KeyCode.G))
            {
                return;
            }

            spawnTimer += Time.deltaTime * spawnPerSec;
            if (spawnTimer >= 1f)
            {
                if (Random.Range(0f, 1f) > .7f)
                {
                    DeepEntity.Create(T_Cube.CubeBig(), Vector2.zero, Quaternion.identity);
                }
                //DeepEntity.Create(T_Cube.Cube(), Vector2.zero, Quaternion.identity);
                spawnTimer -= 1f;
            }
        }
    }
}
