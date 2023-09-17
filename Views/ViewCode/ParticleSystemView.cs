using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction.Views
{
    public class ParticleSystemView : MonoBehaviour
    {
        public DeepViewLink link;
        public ParticleSystem particles;

        private void Awake()
        {
            link.onStartReturn += OnStartReturn;
            link.onSetup += Setup;
        }

        private void Setup()
        {
            link.SetReadyToReturn(false);
        }

        private void OnStartReturn()
        {
            particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            StartCoroutine(WaitForParticles());
        }

        private IEnumerator WaitForParticles()
        {
            while (particles.particleCount > 0)
            {
                yield return null;
            }
            link.SetReadyToReturn(true);
        }
    }
}
