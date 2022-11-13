using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DeepAction
{
    public class DeepViewLink : MonoBehaviour
    {
        //All view code should reference the entity stored in DeepViewLink.
        //There should not be any DeepEntity components on prefabs
        public DeepEntity entity { get; private set; }

        //should the size of this view be considered when determining the hitbox of an entity
        public bool viewAffectsHitbox = false;
        //hitbox size of this view.
        //this value is only sampled when the view is attached.
        [ShowIf("viewAffectsHitbox")]
        public float viewRadius;

        private bool _readyToReturn = true;
        private bool _returning = false;

        private string _viewName;//used to id this view for when we return.

        public Action onSetup;
        public Action onStartReturn;

        public void Setup(DeepEntity e, string viewName)
        {
            entity = e;
            e.views.Add(this);
            _viewName = viewName;
            _returning = false;
            onSetup?.Invoke();
        }

        //Some views will need to linger for a moment after being removed.
        //imagine a view that has a trail. If you return that view as soon as an entity dies it will look really bad. So we let you delay.
        public void StartReturn()
        {
            if (_returning)
            {
                return;
            }
            _returning = true;
            //NOTE that views are removed from the entity while returning. This could maybe cause a bug if your view code does not take _returning into account.
            entity.views.Remove(this);
            onStartReturn?.Invoke();
            if (_readyToReturn)
            {
                Return();
                return;
            }
            //views are unparented if they are not ready to return.
            transform.parent = null;
        }

        /// <summary>
        /// Should only be set by VIEW CODE. Should NEVER be set through a deepBehavior
        /// </summary>
        public void SetReadyToReturn(bool ready)
        {
            _readyToReturn = ready;
            if (_returning)
            {
                Return();
            }
        }

        private void Return()
        {
            DeepViewManager.instance.ReturnView(_viewName, this);
        }

        private void OnDrawGizmosSelected()
        {
            if (!viewAffectsHitbox)
            {
                return;
            }
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, viewRadius);
        }
    }
}
