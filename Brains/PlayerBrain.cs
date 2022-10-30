using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    [RequireComponent(typeof(DeepEntity))]
    public class PlayerBrain : MonoBehaviour
    {
        //the purpose of a brain is to cast behaviors.

        //this scripts is still pretty fuzzy and is likely to change a lot in the future

        private DeepEntity entity;

        private void Awake()
        {
            entity = GetComponent<DeepEntity>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                entity.TryToCast(0);
            }
            if (Input.GetMouseButton(1))
            {
                entity.TryToCast(1);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                entity.TryToCast(2);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                entity.TryToCast(2);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                entity.TryToCast(2);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                entity.TryToCast(2);
            }
        }

    }
}
