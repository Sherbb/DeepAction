using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class ActionOnDeath : DeepBehavior
    {
        private Action[] actions;
        public ActionOnDeath(params Action[] actions)
        {
            this.actions = actions;
        }

        public override void InitializeBehavior()
        {
            parent.events.OnEntityDie += OnDie;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnEntityDie -= OnDie;
        }

        private void OnDie()
        {
            foreach (Action a in actions)
            {
                a.Invoke();
            }
        }
    }
}