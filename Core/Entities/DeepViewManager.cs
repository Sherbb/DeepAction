using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class DeepViewManager : MonoBehaviour
    {
        public static DeepViewManager instance { get; private set; }

        public Dictionary<string, List<GameObject>> viewPool { get; private set; } = new Dictionary<string, List<GameObject>>();

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
                viewPool.Add(view, new List<GameObject>());
            }

            for (int i = 0; i < count; i++)
            {
                GameObject v = Instantiate(g, transform);
                v.SetActive(false);
                viewPool[view].Add(v);
            }
            return true;
        }

        public void ReturnView(string viewName, GameObject gameObject)
        {
            if (!viewPool.ContainsKey(viewName))
            {
                Debug.LogError("View returned without a matching key");
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(false);
            gameObject.transform.parent = transform;
            viewPool[viewName].Add(gameObject);
        }
    }

    public class DeepViewReference
    {
        public GameObject gameObject;
        public string viewName;
        private bool returned;

        public DeepViewReference(GameObject gameObject, string viewName)
        {
            this.gameObject = gameObject;
            this.viewName = viewName;
            returned = false;
        }

        public void ReturnView()
        {
            if (returned)
            {
                return;
            }

            DeepViewManager.instance.ReturnView(viewName, gameObject);
            returned = true;
        }
    }
}
