using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class DeepViewManager : MonoBehaviour
    {
        public static DeepViewManager instance { get; private set; }

        public Dictionary<string, List<DeepViewLink>> viewPool { get; private set; } = new Dictionary<string, List<DeepViewLink>>();

        public Transform inactiveViewParent;
        //holds views that are trying to return, but are waiting on something ex: trails
        public Transform returningViewParent;

        public void Awake()
        {
            instance = this;
            GameObject g = new GameObject();
            g.name = "InactiveViews";
            g.transform.parent = transform;
            inactiveViewParent = g.transform;
            g = new GameObject();
            g.name = "ReturningViews";
            g.transform.parent = transform;
            returningViewParent = g.transform;
        }

        public bool RegisterView(string view, int count = 5)
        {
            GameObject g = Resources.Load(view) as GameObject;

            if (g == null)
            {
                Debug.LogError("Asset for view does note exist: " + view);
                return false;
            }

            if (!viewPool.ContainsKey(view))
            {
                viewPool.Add(view, new List<DeepViewLink>());
            }

            for (int i = 0; i < count; i++)
            {
                GameObject v = Instantiate(g, inactiveViewParent);
                v.SetActive(false);
                DeepViewLink viewLink;
                if (!v.TryGetComponent(out viewLink))
                {
                    viewLink = v.AddComponent<DeepViewLink>();
                }
                viewPool[view].Add(viewLink);
            }
            return true;
        }

        /// <summary>
        /// Returns and INACTIVE view. It will still be parented to viewManager.
        /// </summary>
        public static DeepViewLink PullView(string viewName)
        {
            if (DeepViewManager.instance.viewPool[viewName].Count < 1)
            {
                DeepViewManager.instance.RegisterView(viewName, 1);
            }
            DeepViewLink v = instance.viewPool[viewName][0];
            instance.viewPool[viewName].RemoveAt(0);
            return v;
        }

        public void ReturnView(string viewName, DeepViewLink viewLink)
        {
            if (!viewPool.ContainsKey(viewName))
            {
                Debug.LogError("View returned without a matching key");
                viewLink.gameObject.SetActive(false);
                return;
            }

            viewLink.gameObject.SetActive(false);
            viewLink.transform.parent = inactiveViewParent;
            viewPool[viewName].Add(viewLink);
        }
    }
}
