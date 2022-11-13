using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class PlayerBrain : DeepBehavior
    {
        //the purpose of a brain is to cast behaviors.

        //this scripts is still pretty fuzzy and is likely to change a lot in the future

        public override void InitializeBehavior()
        {
            parent.events.Update += Update;
        }

        public override void DestroyBehavior()
        {
            parent.events.Update -= Update;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                parent.TryToCast(0);
            }
            if (Input.GetMouseButton(1))
            {
                parent.TryToCast(1);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                parent.TryToCast(2);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                parent.TryToCast(2);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                parent.TryToCast(2);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                parent.TryToCast(2);
            }
        }

    }
}
