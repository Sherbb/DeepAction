using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class DeepViewManager : MonoBehaviour
    {
        public static DeepViewManager instance { get; private set; }

        public Dictionary<string, List<DeepViewLink>> viewPool { get; private set; } = new Dictionary<string, List<DeepViewLink>>();

        public void Awake()
        {
            instance = this;
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
                GameObject v = Instantiate(g, transform);
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
            viewLink.transform.parent = transform;
            viewPool[viewName].Add(viewLink);
        }
    }
}
