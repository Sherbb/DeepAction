using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction.Views
{
    public class TrailView : MonoBehaviour
    {
        public DeepViewLink link;
        public TrailRenderer trail;

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
            StartCoroutine(WaitForTrail());
        }

        private IEnumerator WaitForTrail()
        {
            yield return new WaitForSeconds(trail.time);
            link.SetReadyToReturn(true);
        }
    }
}
