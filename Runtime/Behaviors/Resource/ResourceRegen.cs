using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class ResourceRegen : DeepBehavior
    {
        private D_Resource _resource;
        private float _regenPerSec;

        private bool isFull;
        public float timer { get; private set; }//0-1

        public ResourceRegen(D_Resource resource, float regenPerSec)
        {
            _resource = resource;
            _regenPerSec = regenPerSec;
        }

        public override void InitializeBehavior()
        {
            isFull = parent.resources[_resource].isFull;
            parent.resources[_resource].onConsume += CheckResource;
            parent.resources[_resource].onFill += CheckResource;
            parent.events.UpdateNorm += Update;
        }

        public override void DestroyBehavior()
        {
            parent.resources[_resource].onConsume -= CheckResource;
            parent.resources[_resource].onFill -= CheckResource;
            parent.events.UpdateNorm -= Update;
        }

        //we restart the timer when we go from full => empty
        //we also need to know when we are full.
        private void CheckResource()
        {
            bool wasFull = isFull;
            isFull = parent.resources[_resource].isFull;

            if (wasFull && !isFull)
            {
                timer = 0f;
            }
        }

        //todo you can optimize this update call by enabling it based on isFull
        private void Update()
        {
            if (isFull)
            {
                return;
            }

            timer += Time.deltaTime * _regenPerSec;

            while (timer >= 1f)
            {
                parent.resources[_resource].Regen(1);
                timer -= 1f;
            }
        }
    }
}