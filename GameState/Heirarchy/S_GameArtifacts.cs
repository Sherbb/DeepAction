using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    [System.Serializable]
    public class S_GameArtifacts
    {
        public DeepState<float> artifactBuildup = new DeepState<float>(0f);//every time this reaches > 1 an artifact is created.

        public void ArtifactBuildup(float value, Vector3 position)
        {
            artifactBuildup.SetValue(artifactBuildup.value + value);
            while (artifactBuildup.value >= 1f)
            {
                //DeepEntity.Create(T_Artifacts.Artifact(), position, Quaternion.identity);
                artifactBuildup.SetValue(artifactBuildup.value - 1f);
            }
        }
    }
}
