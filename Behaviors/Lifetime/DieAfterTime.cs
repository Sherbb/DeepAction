using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class DieAfterTime : DeepBehavior
    {
        public float time;
        private bool waiting;
        private Coroutine coroutine;

        public DieAfterTime(float time)
        {
            this.time = time;
        }

        public override void InitializeBehavior()
        {
            waiting = false;
            coroutine = parent.StartCoroutine(WaitCo());
        }

        public override void DestroyBehavior()
        {
            if (waiting)
            {
                parent.StopCoroutine(coroutine);
            }
        }

        private IEnumerator WaitCo()
        {
            yield return new WaitForSeconds(time);
            waiting = false;
            parent.Die();
        }
    }
}